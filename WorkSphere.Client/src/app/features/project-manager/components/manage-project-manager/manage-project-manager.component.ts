import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Subscription} from 'rxjs';
import {ToastService} from '../../../../services/toast.service';
import {DatePipe} from '@angular/common';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {NgForm, NgModel} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';
import {ProjectManager} from '../../model/project-manager';
import {ProjectManagerService} from '../../services/project-manager.service';
import {inputDateFormat} from '../../../../shared/utils/date-utils';

@Component({
  selector: 'app-manage-project-manager',
  standalone: false,

  templateUrl: './manage-project-manager.component.html',
  styleUrl: './manage-project-manager.component.css'
})
export class ManageProjectManagerComponent implements OnInit, OnDestroy {
  @Input() visible: boolean = false;
  @Input() projectManagerId: number | null = null;
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() projectManagerSaved: EventEmitter<null> = new EventEmitter<null>();

  title: String = ''
  message: string = '';
  saveMessage: string = '';
  mode: string = 'add';
  buttonLoading: boolean = false;
  projectManager: ProjectManager = new ProjectManager(
    null,
    '',
    '',
    '',
    new Date(),
  );
  private addProjectManagerSubscription?: Subscription
  private getProjectManagerSubscription?: Subscription
  private updateProjectManagerSubscription?: Subscription

  constructor(
    private toastService: ToastService,
    private datePipe: DatePipe,
    private projectManagerService: ProjectManagerService,
    private errorHandlingService: ErrorHandlerService
  ) {
  }

  ngOnInit(): void {
    if (!this.projectManagerId) {
      this.title = 'Add Project Manager'
    } else {
      this.title = 'Edit Project Manager'
      this.mode = 'edit';
      this.getProjectManagerById(this.projectManagerId.toString());
    }
    this.projectManager.employmentDate = inputDateFormat(this.projectManager.employmentDate, this.datePipe);
  }

  /**
   * Validate the date to ensure it is not in the future
   * If the date is in the future, reset it to today
   */
  validateDate(date: NgModel) {
    if (!date.viewModel) return; // Ensure there's a value to validate

    // Convert the selected date string to a UTC date (avoiding time zone issues)
    const selectedDate = new Date(date.viewModel + "T00:00:00Z");
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Normalize today's date to avoid time differences

    if (selectedDate > today) {
      date.control.setErrors({maxDate: true}); // Mark as invalid
    } else {
      date.control.setErrors(null); // Clear the error if valid
    }
  }

  onFormSubmit(form: NgForm) {
    this.message = '';
    this.saveMessage = '';

    //if the form is not valid, we can return
    if (form.invalid) {
      this.message = 'Please correct the validation errors';
      console.log(this.message)
      return;
    }
    if (this.mode === 'add') {
      this.addProjectManagerSubscription = this.projectManagerService.createProjectManager(this.projectManager).subscribe({
        next: projectManager => {
          console.log(projectManager)
          this.saveMessage = `ProjectManager saved with Id: ${projectManager.id}`;
          this.toastService.showSuccess(`ProjectManager created with Id: ${projectManager.id}`)
          this.buttonLoading = false;
          this.projectManagerSaved.emit();
          this.onClose();
        }
        ,
        error: (err: HttpErrorResponse) => {
          this.errorHandlingService.apiErrorHandler(err);
          this.buttonLoading = false;
        },
        complete: () => {
          this.buttonLoading = false;
        }
      })

    } else {
      this.updateProjectManagerSubscription = this.projectManagerService.updateProjectManager(this.projectManager).subscribe({
          next: projectManager => {
            this.toastService.showSuccess(`ProjectManager saved`)
            this.buttonLoading = false;
            this.projectManagerSaved.emit();
            this.onClose();
          },
          error: (err: HttpErrorResponse) => {
            this.errorHandlingService.apiErrorHandler(err);
            this.buttonLoading = false;
          },
          complete: () => {
            this.buttonLoading = false;
          }
        }
      )
    }
  }

  onClose() {
    this.visible = false;
    this.visibilityChange.emit(false);
  }

  ngOnDestroy(): void {
    if (this.addProjectManagerSubscription) {
      this.addProjectManagerSubscription.unsubscribe()
    }
    if (this.getProjectManagerSubscription) {
      this.getProjectManagerSubscription.unsubscribe()
    }
    if (this.updateProjectManagerSubscription) {
      this.updateProjectManagerSubscription.unsubscribe()
    }
  }

  private getProjectManagerById(id: string): void {
    this.getProjectManagerSubscription = this.projectManagerService.getProjectManager(id).subscribe({
      next: projectManager => {
        if (projectManager) {
          projectManager.employmentDate = this.datePipe.transform(projectManager.employmentDate, 'yyyy-MM-dd');
          this.projectManager = projectManager;
        } else {
          this.toastService.showError('Project Manager not found')
        }
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandlingService.apiErrorHandler(err);
      }
    })
  }
}
