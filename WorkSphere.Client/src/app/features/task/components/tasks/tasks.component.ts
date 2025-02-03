import {Component, OnDestroy, OnInit} from '@angular/core';
import {ConfirmationService, MenuItem, MessageService} from 'primeng/api';
import {ProjectService} from '../../../project/services/project.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {TaskService} from '../../services/task.service';
import {Employee} from '../../../employees/model/employee';
import {Status, Task} from '../../model/task';
import {ToastService} from '../../../../services/toast.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

interface ProjectOption {
  id: number;
  name: string;
}

type TaskComponent = {
  [key in Status]: Task[]
}

enum StatusEnum {
  Active = 1,
  Inactive = 2,
  Completed = 3
}

@Component({
  selector: 'app-tasks',
  standalone: false,
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.css',

})
export class TasksComponent implements OnInit, OnDestroy {
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};

  readonly header = "Tasks";
  projectSelectionOptions: ProjectOption[] = [];
  selectedProject: ProjectOption | undefined
  selectedProjectConfirmed: boolean = false;
  buttonLoading: boolean = false;
  getProjectsSubscription?: Subscription
  getTaskSubscription?: Subscription
  updateTaskSubscription?: Subscription
  teamMembers: Employee[] = [];
  activeTasks: Task[] = [];
  inactiveTasks: Task[] = [];
  completedTasks: Task[] = [];
  finalTasks: Task[] = [];

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
    this.updateTaskSubscription?.unsubscribe()
  }

  drop(event: CdkDragDrop<Task[]>) {
    console.log(event);
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
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

  handleTaskSubmit() {
    //convert activer, inactive, completed tasks to final tasks with status as enum value
    this.finalTasks = this.activeTasks.map(task => ({
      ...task,
      status: 'Active' as Status
    })).concat(this.inactiveTasks.map(task => ({
      ...task,
      status: 'Inactive' as Status
    }))).concat(this.completedTasks.map(task => ({
      ...task,
      status: 'Completed' as Status
    })));
    this.updateTask();

  }

  private getTasks = (projectId: number) => {
    this.getTaskSubscription = this.taskService.getTasks(projectId)
      .subscribe({
        next: (apiResponse) => {
          if (apiResponse.projectTasks.length > 0) {
            //group tasks by status
            const taskComponent: TaskComponent = {
              'Inactive': [],
              'Active': [],
              'Completed': []
            }
            for (const task of apiResponse.projectTasks) {
              taskComponent[task.status as Status].push(task);
            }
            this.activeTasks = taskComponent['Active'];
            this.inactiveTasks = taskComponent['Inactive'];
            this.completedTasks = taskComponent['Completed'];


          }
          this.teamMembers = apiResponse.projectTeamMembers || [];
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      });
  }

  private updateTask() {
    this.buttonLoading = true;
    this.updateTaskSubscription = this.taskService.updateTasks(this.finalTasks, this.selectedProject.id).subscribe({
      next: () => {
        this.toastService.showSuccess('Tasks updated successfully');
        this.buttonLoading = false;
      },
      error: (error) => {
        this.buttonLoading = false;
        this.errorHandlerService.apiErrorHandler(error);
      },

    })
  }
}
