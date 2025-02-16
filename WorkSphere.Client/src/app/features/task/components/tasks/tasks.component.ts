import {Component, OnDestroy, OnInit} from '@angular/core';
import {MenuItem} from 'primeng/api';
import {ProjectService} from '../../../project/services/project.service';
import {Subscription} from 'rxjs';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {TaskService} from '../../services/task.service';
import {Employee} from '../../../employees/model/employee';
import {Status, Task} from '../../model/task';
import {ToastService} from '../../../../services/toast.service';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import {NgForm, NgModel} from '@angular/forms';
import {EmployeeService} from '../../../employees/services/employee.service';

interface ProjectOption {
  id: number;
  name: string;
}

type TaskComponent = {
  [key in Status]: Task[]
}

const defaultTask: Task = {
  id: 0,
  name: '',
  description: '',
  status: 'Inactive',
  order: 0,
  duration: 1,
  projectID: 0,
  employeeIDs: []
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
  mode: string = 'Add';
  projectSelectionOptions: ProjectOption[] = [];
  selectedProject: ProjectOption | undefined
  selectedProjectConfirmed: boolean = false;
  buttonLoading: boolean = false;
  getProjectsSubscription?: Subscription
  teamMembers: Employee[] = [];
  activeTasks: Task[] = [];
  inactiveTasks: Task[] = [];
  completedTasks: Task[] = [];
  finalTasks: Task[] = [];
  isLoading: boolean = false;
  showModal: boolean = false
  task: Task = {...defaultTask}
  availableTeamMembers: Array<Employee> = [];
  private originalTask: Task | null = null;

  private currentOrder: number = 1;
  private subscriptions: Subscription[] = [];

  constructor(
    private projectService: ProjectService,
    private taskService: TaskService,
    private errorHandlerService: ErrorHandlerService,
    private toastService: ToastService,
    private employeeService: EmployeeService
  ) {
  }

  ngOnInit() {
    this.subscriptions.push(
      this.projectService.projectResponse$.subscribe((response) => {
        if (response) {
          const projects = response.projects;
          this.projectSelectionOptions = projects.map((project) => {
            return {id: project.id, name: project.name};
          });
        }
      })
    )
    this.subscriptions.push(
      this.employeeService.employeeResponse$.subscribe((response) => {
        if (response) {
          this.availableTeamMembers = response.employees;
        }
      })
    )
    this.projectService.refreshProjects(0, 40);
    this.employeeService.getEmployees(0, 40);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    this.getProjectsSubscription?.unsubscribe();
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

  handleProjectSelected() {
    if (!this.selectedProject) {
      this.toastService.showError('Please select a project', 'Error');
      this.selectedProjectConfirmed = false;
      return;
    }
    this.isLoading = true;
    this.selectedProjectConfirmed = true;
    this.getTasks(this.selectedProject.id);
  }

  handleTaskSubmit() {
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
    //if id is 0 remove it
    this.finalTasks = this.finalTasks.map(task => {
      if (task.id === 0) {
        delete task.id;
      }
      return task;
    })
    this.updateTask();
  }

  onFormSubmit(form: NgForm) {
    if (form.invalid) {
      this.toastService.showError('Please correct the validation errors');
      return;
    }
    if (this.mode === 'Add') {
    } else {
      // Update the task in the corresponding task list
      let taskList: Task[] = [];
      switch (this.task.status) {
        case 'Active':
          taskList = this.activeTasks;
          break;
        case 'Inactive':
          taskList = this.inactiveTasks;
          break;
        case 'Completed':
          taskList = this.completedTasks;
          break;
      }
      const index = taskList.findIndex(t => t.id === this.task.id);
      if (index > -1) {
        taskList[index] = {...this.task};
      }
      this.showModal = false;

    }
  }

  updateTask() {
    this.buttonLoading = true;
    this.subscriptions.push(
      this.taskService.updateTasks(this.finalTasks, this.selectedProject.id).subscribe({
          next: () => {
            this.toastService.showSuccess('Tasks updated successfully');
            this.buttonLoading = false;
          },
          error: (error) => {
            this.buttonLoading = false;
            this.errorHandlerService.apiErrorHandler(error);
          },

        }
      ));
  }

  validateDuration(duration: NgModel) {
    if (!duration.viewModel) return;
    if (duration.viewModel <= 0) {
      duration.control.setErrors({durationError: true});
    } else {
      duration.control.setErrors(null);
    }
  }

  deleteTask(task: Task, status: string) {
    let taskList: Task[] = [];
    switch (status) {
      case 'Active':
        taskList = this.activeTasks;
        break;
      case 'Inactive':
        taskList = this.inactiveTasks;
        break;
      case 'Completed':
        taskList = this.completedTasks;
        break;
    }
    const index = taskList.findIndex(t => t.id === task.id);
    if (index > -1) {
      taskList.splice(index, 1);
    }
  }

  showAddTaskModal() {
    this.showModal = true;
    this.mode = 'Add';
    this.task = {...defaultTask};
  }

  showEditTaskModal(item: Task) {
    this.showModal = true;
    this.mode = 'Edit';
    this.task = {...item};
    this.originalTask = {...item};
  }

  closeModal(task: Task) {
    if (this.originalTask) {
      Object.assign(task, this.originalTask);
      this.originalTask = null;
    }
    this.showModal = false;
  }

  private getTasks = (projectId: number) => {
    this.subscriptions.push(
      this.taskService.getTasks(projectId)
        .subscribe({
            next: (apiResponse) => {
              if (apiResponse.projectTasks.length > 0) {
                //group tasks by status
                const taskComponent: TaskComponent = {
                  'Inactive': [],
                  'Active': [],
                  'Completed': []
                };
                //set employeeIDs to array
                for (const task of apiResponse.projectTasks) {
                  taskComponent[task.status as Status].push(task);
                }
                this.activeTasks = taskComponent['Active'];
                this.inactiveTasks = taskComponent['Inactive'];
                this.completedTasks = taskComponent['Completed'];
              } else {
                this.activeTasks = [];
                this.inactiveTasks = [];
                this.completedTasks = [];
              }
              this.isLoading = false;
              this.task.projectID = projectId;
              this.teamMembers = apiResponse.projectTeamMembers || [];
            },
            error: (error) => {
              this.errorHandlerService.apiErrorHandler(error);
              this.isLoading = false;
            }
          }
        ));
  }
}
