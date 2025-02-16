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
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css',
  standalone: false
})
export class ProjectsComponent implements OnInit, OnDestroy {
  isLoading = true;
  showAddProject = false;
  showDetailsProject = false;
  selectedProjectId: number | null = null;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  header = "Projects";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;
  projects: Project[] = [];
  private subscriptions: Subscription[] = [];

  constructor(
    private projectService: ProjectService,
    private errorHandlerService: ErrorHandlerService,
    private confirmService: ConfirmationService,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.handleRouteProjectSelection();
    this.subscriptions.push(
      this.projectService.projectResponse$.subscribe((response) => {
        if (response) {
          this.projects = response.projects;
          this.totalRecords = response.totalCount;
          this.pageIndex = response.pageIndex;
          this.pageSize = response.pageSize;
          this.isLoading = false;
        }
      })
    );
    this.projectService.getProjects(this.pageIndex, this.pageSize);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  openDrawer(projectId?: number) {
    this.selectedProjectId = projectId || null;
    this.showAddProject = true;
  }

  prevPage() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.projectService.getProjects(this.pageIndex, this.pageSize);
    }
  }

  nextPage() {
    if ((this.pageIndex + 1) * this.pageSize < this.totalRecords) {
      this.pageIndex++;
      this.projectService.getProjects(this.pageIndex, this.pageSize);
    }
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddProject = isVisible;
    if (!isVisible) this.projectService.getProjects(this.pageIndex, this.pageSize);
  }

  confirmDelete(event: Event, projectId: number): void {
    this.confirmService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this project?',
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      rejectLabel: 'Cancel',
      rejectButtonProps: {label: 'Cancel', severity: 'secondary', outlined: true},
      acceptButtonProps: {label: 'Delete', severity: 'danger'},
      accept: () => this.deleteProject(projectId)
    });
  }

  navigateToProjectDetails(projectId: number): void {
    this.router.navigate(['/projects'], {queryParams: {id: projectId}});
  }

  deleteProject(projectId: number): void {
    this.subscriptions.push(
      this.projectService.deleteProject(projectId).subscribe({
        next: () => {
          this.toastService.showSuccess('Project Deleted');
          this.projectService.refreshProjects(this.pageIndex, this.pageSize);
        },
        error: (error) => this.errorHandlerService.apiErrorHandler(error)
      })
    );
  }

  refetchProjects() {
    this.projectService.refreshProjects(0, 10);
  }

  private handleRouteProjectSelection(): void {
    this.subscriptions.push(
      this.route.queryParams.subscribe(params => {
        const projectId = parseInt(params['id'], 10);
        console.log("projectId", projectId);
        if (projectId) {
          this.selectedProjectId = projectId;
          this.showDetailsProject = true;
          this.items = [
            {label: 'projects', url: '/projects'},
            {label: `${projectId}`, url: '/projects', queryParams: {id: projectId}}
          ];
        } else {
          this.showDetailsProject = false;
          this.items = [{label: 'projects', url: '/projects'}];
          this.projectService.getProjects(this.pageIndex, this.pageSize);
        }
      })
    );
  }
}
