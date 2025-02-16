import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {NgForm, NgModel} from '@angular/forms';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {ToastService} from '../../../../services/toast.service';
import {DatePipe} from '@angular/common';
import {Subscription} from 'rxjs';
import {inputDateFormat} from '../../../../shared/utils/date-utils';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

const defaultProject = new Project(
  null,
  '',
  '',
  new Date(),
  new Date(),
  0,
);

@Component({
  selector: 'app-manage-project',
  standalone: false,
  templateUrl: './manage-project.component.html',
  styleUrl: './manage-project.component.css'
})
export class ManageProjectComponent implements OnInit, OnDestroy {

  //region Properties
  @Input() visible: boolean = false;
  @Input() projectId: number | null = null;
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() projectSaved: EventEmitter<null> = new EventEmitter<null>();

  title: string = 'Add Project';
  project: Project = {...defaultProject};
  buttonLoading: boolean = false;
  mode: string = 'add';

  getProjectSubscription?: Subscription
  addProjectSubscription?: Subscription
  updateProjectSubscription?: Subscription
  //endregion

  //region Constructor
  constructor(
    private projectService: ProjectService,
    private toastService: ToastService,
    private datePipe: DatePipe,
    private errorHandlerService: ErrorHandlerService,
  ) {
  }

  //endregion

  //region Lifecycle Hooks
  /**
   * @description This method is called when the component is initialized.
   */
  ngOnInit(): void {
    if (this.projectId) {
      this.loadProject();
      this.title = 'Edit Project';
      this.mode = 'edit';
    }
    this.project.startDate = inputDateFormat(this.project.startDate, this.datePipe);
    this.project.endDate = inputDateFormat(this.project.endDate, this.datePipe);
  }


  ngOnDestroy(): void {
    if (this.getProjectSubscription) {
      this.getProjectSubscription.unsubscribe()
    }
    if (this.addProjectSubscription) {
      this.addProjectSubscription.unsubscribe()
    }
    if (this.updateProjectSubscription) {
      this.updateProjectSubscription.unsubscribe()
    }

    this.projectSaved.unsubscribe();
    this.visibilityChange.unsubscribe();
    this.projectId = null;
  }

  //endregion

  //region Public Methods
  /**
   * This method is called when the close button is clicked.
   * It emits the visibilityChange event with false.
   * It also sets the visible property to false.
   */
  onClose() {
    this.visibilityChange.emit(false);
    this.visible = false;
  }

  /**
   * This method is called when the form is submitted.
   * @param projectForm
   */
  onFormSubmit(projectForm: NgForm) {
    if (!projectForm.valid) {
      return;
    }
    this.buttonLoading = true;
    if (this.mode === 'add') {
      this.addProject();
    } else {
      this.updateProject();
    }
  }

  /**
   * This method is called when the start date or end date is changed.
   * It validates the date and sets the error if the date is invalid.
   * @param date - The date input field.
   */
  validateDate(date: NgModel) {
    const label = date.name
    const selectedDate = new Date(date.viewModel + "T00:00:00Z"); // Convert to date
    const startDate = new Date(this.project.startDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    if (label === 'startDate') {
      if (selectedDate < today) {
        date.control.setErrors({dateError: true}); // Mark as invalid
      }
    } else if (label === 'endDate') {
      if (selectedDate < startDate) {
        date.control.setErrors({dateError: true}); // Mark as invalid
      }
    }
  }

  /**
   * This method is called when the project is loaded.
   */
  loadProject() {
    this.getProjectSubscription = this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.project.startDate = inputDateFormat(this.project.startDate, this.datePipe);
        this.project.endDate = inputDateFormat(this.project.endDate, this.datePipe);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  /**
   * This method is called when the project is added.
   * @private
   */
  private addProject() {
    this.addProjectSubscription = this.projectService.creatProject(this.project).subscribe({
      next: project => {
        this.toastService.showSuccess(`Project created with Id: ${project.id}`);
        this.projectSaved.emit();
        this.onClose();
        this.buttonLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.errorHandlerService.apiErrorHandler(err);
        this.buttonLoading = false;
      }
    });
  }

  private updateProject() {
    this.updateProjectSubscription = this.projectService.updateProject(this.project).subscribe({
      next: project => {
        this.toastService.showSuccess(`Project updated `);
        this.projectSaved.emit();
        this.onClose();
        this.buttonLoading = false;
      },
      error: (err) => {
        this.errorHandlerService.apiErrorHandler(err);
        this.buttonLoading = false;
      }
    });
  }
}
