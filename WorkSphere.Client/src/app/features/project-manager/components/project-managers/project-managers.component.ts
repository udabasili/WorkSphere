import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {firstValueFrom, Subject, Subscription, takeUntil} from 'rxjs';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastService} from '../../../../services/toast.service';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {ProjectManager} from '../../model/project-manager';
import {ProjectManagerService} from '../../services/project-manager.service';

@Component({
  selector: 'app-project-managers',
  standalone: false,

  templateUrl: './project-managers.component.html',
  styleUrl: './project-managers.component.css'
})
export class ProjectManagersComponent implements OnInit, OnDestroy {
  @Input() visible = false;

  projectManagers: ProjectManager[] = [];
  isLoading = true;
  showAddProjectManager = false;
  showDetailsProjectManager: boolean = false
  selectedProjectManagerId: number | null = null;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  readonly header = "Project Managers";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;

  private deleteProjectManagerSubscription?: Subscription;
  private routeDestroy$ = new Subject<void>();

  constructor(
    private projectManagerService: ProjectManagerService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  ngOnInit(): void {
    this.handleRouteProjectManagerSelection();
  }

  ngOnDestroy(): void {
    this.routeDestroy$.next(); // Unsubscribe from all observables
    this.routeDestroy$.complete(); // Complete the subject
    if (this.deleteProjectManagerSubscription) {
      this.deleteProjectManagerSubscription.unsubscribe();
    }
    this.showAddProjectManager = false;
    this.showDetailsProjectManager = false;
    this.selectedProjectManagerId = null;

  }

  openDrawer(projectManagerId?: number) {
    if (projectManagerId) {
      this.selectedProjectManagerId = projectManagerId;
    } else {
      this.selectedProjectManagerId = null;
    }
    this.showAddProjectManager = true;
  }

  prevPage() {
    this.pageIndex = this.pageIndex - 1;
    this.loadProjectManagers();
  }

  nextPage() {
    this.pageIndex = this.pageIndex + 1;
    this.loadProjectManagers();
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddProjectManager = isVisible;
    if (!isVisible) {
      this.loadProjectManagers(

      ); // Reload projectManagers when the drawer is closed
    }
  }

  async loadProjectManagers() {
    try {
      //get all projectManagers
      //firstValueFrom is used to convert an observable to a promise
      const response = await firstValueFrom(this.projectManagerService.getProjectManagers(this.pageIndex, this.pageSize));
      this.projectManagers = response.projectManagers;
      this.pageIndex = response.pageIndex;
      this.pageSize = response.pageSize;
      this.totalRecords = response.totalCount;
      this.isLoading = false;
    } catch (error: any) {
      this.errorHandlerService.apiErrorHandler(error);
      this.isLoading = false;

    }
  }

  handleProjectManagerSelected(projectManager: ProjectManager): void {
    this.router.navigate(['/project-managers'], {queryParams: {id: projectManager.id}});
  }

  confirmDelete(event: Event, projectManagerId: number): void {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this Project Manager?',
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      rejectLabel: 'Cancel',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },
      accept: () => {
        this.deleteProjectManager(projectManagerId);
      },
    });
  }


  /**
   * Handles the route project selection
   * If a project id is present in the route, it will show the project details
   * Otherwise, it will show all projects
   * @private
   */
  private handleRouteProjectManagerSelection(): void {
    this.route.queryParams.pipe(takeUntil(this.routeDestroy$)).subscribe(params => {
      const projectManagerId = parseInt(params['id'], 10);
      if (projectManagerId) {
        this.selectedProjectManagerId = projectManagerId;
        this.showDetailsProjectManager = true
        this.items = [{label: 'Project Managers', url: '/project-managers'}, {
          label: `${projectManagerId}`,
          url: '/project-managers',
          queryParams: {id: projectManagerId}
        }]

      } else {
        this.loadProjectManagers();
        this.showDetailsProjectManager = false
        this.items = [{label: 'Project Managers', url: '/project-managers'}]

      }
    });
  }

  private deleteProjectManager(projectManagerId: number): void {
    this.deleteProjectManagerSubscription = this.projectManagerService.deleteProjectManager(projectManagerId).subscribe({
      next: () => {
        this.toastService.showSuccess('ProjectManager Deleted');
        this.pageIndex = 0;
        this.loadProjectManagers();
      },
      error: (error) => {
        this.errorHandlerService.apiErrorHandler(error);
      }
    });
  }
}
