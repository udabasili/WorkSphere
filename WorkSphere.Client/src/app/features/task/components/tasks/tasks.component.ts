import {Component, OnDestroy, OnInit} from '@angular/core';
import {ConfirmationService, MenuItem, MessageService} from 'primeng/api';
import {ProjectService} from '../../../project/services/project.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {TaskService} from '../../services/task.service';
import {Employee} from '../../../employees/model/employee';
import {Task} from '../../model/task';
import {ToastService} from '../../../../services/toast.service';

interface ProjectOption {
  id: number;
  name: string;
}

@Component({
  selector: 'app-tasks',
  standalone: false,
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.css'
})
export class TasksComponent implements OnInit, OnDestroy {
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  readonly header = "Tasks";
  projectSelectionOptions: ProjectOption[] = [];
  selectedProject: ProjectOption | undefined
  selectedProjectConfirmed: boolean = false;

  getProjectsSubscription?: Subscription
  getTaskSubscription?: Subscription
  teamMembers: Employee[] = [];
  tasks: Task[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private projectService: ProjectService,
    private taskService: TaskService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private errorHandlerService: ErrorHandlerService,
    private toastService: ToastService
  ) {
  }

  ngOnInit() {
    this.getProjects();
  }

  ngOnDestroy() {
    this.getProjectsSubscription?.unsubscribe();
  }

  getProjects() {
    this.getProjectsSubscription = this.projectService.getProjects(0, 30)
      .subscribe({
        next: (apiResponse) => {
          const projects = apiResponse.projects;
          this.projectSelectionOptions = projects.map((project) => {
            return {id: project.id, name: project.name};
          });
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      });
  }

  handleProjectSelected() {
    console.log('handleProjectSelected', !this.selectedProject)
    if (!this.selectedProject) {
      this.toastService.showError('Please select a project', 'Error');
      this.selectedProjectConfirmed = false;
      return;
    }
    this.selectedProjectConfirmed = true;
    this.getTasks(this.selectedProject.id);
  }

  addTeam() {

  }

  private getTasks = (projectId: number) => {
    this.getTaskSubscription = this.taskService.getTasks(projectId)
      .subscribe({
        next: (apiResponse) => {
          this.tasks = apiResponse.projectTasks || [];
          this.teamMembers = apiResponse.projectTeamMembers || [];
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      });
  }
}
