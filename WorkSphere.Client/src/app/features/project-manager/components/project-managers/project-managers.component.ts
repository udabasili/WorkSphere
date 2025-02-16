import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {Subject, Subscription, takeUntil} from 'rxjs';
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
  header = "Project Managers";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;

  private subscriptions: Subscription[] = [];

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
    this.projectManagerService.projectManagerResponse$.subscribe((response) => {
      if (response) {
        this.projectManagers = response.projectManagers;
        this.totalRecords = response.totalCount;
        this.pageIndex = response.pageIndex;
        this.pageSize = response.pageSize;
        this.isLoading = false;
      }
    })
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
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
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.projectManagerService.getProjectManagers(this.pageIndex, this.pageSize);
    }
  }

  nextPage() {
    if ((this.pageIndex + 1) * this.pageSize < this.totalRecords) {
      this.pageIndex++;
      this.projectManagerService.getProjectManagers(this.pageIndex, this.pageSize);
    }
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddProjectManager = isVisible;
    if (!isVisible) {
      this.projectManagerService.getProjectManagers(this.pageIndex, this.pageSize);
    }
  }


  handleProjectManagerSelected(projectManager: ProjectManager): void {
    this.router.navigate(['/project-managers'], {queryParams: {id: projectManager.id}});
  }

  confirmDelete(event: Event, projectManagerId: number): void {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this projectManager?',
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

  refetchProjectManagers() {
    this.projectManagerService.refetchProjectManagers(0, 10);
  }

  /**
   * Handles the route project selection
   * If a project id is present in the route, it will show the project details
   * Otherwise, it will show all projects
   * @private
   */
  private handleRouteProjectManagerSelection(): void {
    this.subscriptions.push(
      this.route.queryParams.pipe(takeUntil(new Subject())).subscribe({
        next: async (params) => {
          if (params['id']) {
            const projectManagerId = parseInt(params['id'], 10);
            this.selectedProjectManagerId = projectManagerId;
            this.showDetailsProjectManager = true;
            this.selectedProjectManagerId = projectManagerId;
            this.showDetailsProjectManager = true
            this.items = [{label: 'Project Managers', url: '/project-managers'}, {
              label: `${projectManagerId}`,
              url: '/project-managers',
              queryParams: {id: projectManagerId}
            }]

          } else {
            this.projectManagerService.getProjectManagers(this.pageIndex, this.pageSize);
            this.showDetailsProjectManager = false
            this.items = [{label: 'ProjectManagers', url: '/project-managers'}]

          }
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

  private deleteProjectManager(projectManagerId: number): void {
    this.subscriptions.push(
      this.projectManagerService.deleteProjectManager(projectManagerId).subscribe({
        next: () => {
          this.toastService.showSuccess('ProjectManager Deleted');
          this.pageIndex = 0;
          this.refetchProjectManagers();
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

}
