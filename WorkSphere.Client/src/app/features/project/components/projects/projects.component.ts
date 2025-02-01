import {Component, OnDestroy, OnInit} from '@angular/core';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {Subscription} from 'rxjs';
import {MenuItem} from 'primeng/api';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

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

  getProjectSubscription: Subscription

  constructor(
    private projectService: ProjectService,
    private errorHandlerService: ErrorHandlerService
  ) {
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

  ngOnInit(): void {
    this.loadProjects()
  }

  ngOnDestroy(): void {
    if (this.getProjectSubscription) {
      this.getProjectSubscription.unsubscribe()
    }
    this.showAddProject = false;

  }

  loadProject() {

  }

  private loadProjects() {
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


}
