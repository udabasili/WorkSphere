import {Component, OnDestroy, OnInit} from '@angular/core';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {Subscription} from 'rxjs';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {ToastService} from '../../../../services/toast.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-projects',
  standalone: false,

  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent implements OnInit, OnDestroy {

  isLoading = true;
  showAddProject = false;
  showDetailsProject: boolean = false
  selectedProjectId: number | null = null;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  header = "Projects";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;
  projects: Array<Project> = []

  private getProjectSubscription: Subscription
  private deleteProjectSubscription?: Subscription
  private routerSubscription?: Subscription

  constructor(
    private projectService: ProjectService,
    private errorHandlerService: ErrorHandlerService,
    private confirmService: ConfirmationService,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
  }

  ngOnInit(): void {
    this.handleRouteProjectSelection()
  }

  ngOnDestroy(): void {
    if (this.getProjectSubscription) {
      this.getProjectSubscription.unsubscribe()
    }
    if (this.deleteProjectSubscription) {
      this.deleteProjectSubscription.unsubscribe()
    }
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe()
    }
    this.showAddProject = false;
  }


  openDrawer(projectId?: number) {
    if (projectId) {
      this.selectedProjectId = projectId;
    } else {
      this.selectedProjectId = null;
    }
    this.showAddProject = true;
  }

  prevPage() {
    this.pageIndex = this.pageIndex - 1;
    this.loadProjects();
  }

  nextPage() {
    this.pageIndex = this.pageIndex + 1;
    this.loadProjects();
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddProject = isVisible;
    if (!isVisible) {
      this.loadProjects(
      );
    }
  }

  confirmDelete(event: Event, projectId: number): void {
    this.confirmService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this project?',
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
        this.deleteProject(projectId);
      },
    });
  }

  navigateToProjectDetails(projectId: number): void {
    this.router.navigate(['/projects'], {queryParams: {id: projectId}});
  }

  deleteProject(projectId: number): void {
    this.deleteProjectSubscription = this.projectService.deleteProject(projectId).subscribe({
      next: () => {
        this.toastService.showSuccess('Project Deleted');
        //go back to the first page if the current page has no records
        this.pageIndex = 0;
        this.loadProjects();
      },
      error: (error) => {
        this.errorHandlerService.apiErrorHandler(error);
      }
    });
  }

  loadProjects() {
    this.getProjectSubscription = this.projectService.getProjects(this.pageIndex, this.pageSize).subscribe({
      next: (apiResponse) => {
        this.projects = apiResponse.projects
        this.totalRecords = apiResponse.totalCount
        this.pageIndex = apiResponse.pageIndex
        this.pageSize = apiResponse.pageSize

        this.isLoading = false
      },
      error: (error) => {
        this.errorHandlerService.apiErrorHandler(error);
        this.isLoading = false
      }
    })
  }

  closeLoader() {
    this.isLoading = false;

  }

  /**
   * Handles the route project selection
   * If a project id is present in the route, it will show the project details
   * Otherwise, it will show all projects
   * @private
   */
  private handleRouteProjectSelection(): void {
    this.routerSubscription = this.route.queryParams.subscribe(params => {
      const projectId = parseInt(params['id'], 10);
      if (projectId) {
        this.selectedProjectId = projectId;
        this.showDetailsProject = true;
        this.items = [
          {label: 'Projects', url: '/projects'},
          {label: `${projectId}`, url: '/projects', queryParams: {id: projectId}}
        ];

      } else {
        this.loadProjects();
        this.showDetailsProject = false;
        this.items = [{label: 'Projects', url: '/projects'}];
      }
    });
  }
}
