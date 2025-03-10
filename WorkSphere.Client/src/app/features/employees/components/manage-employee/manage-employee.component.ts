import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Employee} from '../../model/employee';
import {DatePipe} from '@angular/common';
import {NgForm, NgModel} from '@angular/forms';
import {Subscription} from 'rxjs';
import {EmployeeService} from '../../services/employee.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ToastService} from '../../../../services/toast.service';
import {inputDateFormat} from '../../../../shared/utils/date-utils';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

const defaultEmployee = new Employee(
  null,
  '',
  '',
  '',
  new Date(),
);

@Component({
  selector: 'app-manage-employee',
  standalone: false,
  templateUrl: './manage-employee.component.html',
  styleUrl: './manage-employee.component.css'
})

export class ManageEmployeeComponent implements OnInit, OnDestroy {
  //region Properties
  @Input() visible: boolean = false;
  @Input() employeeId: number | null = null;
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() employeeSaved: EventEmitter<null> = new EventEmitter<null>();

  title: String = ''
  message: string = '';
  saveMessage: string = '';
  mode: string = 'add';
  buttonLoading: boolean = false;
  employee: Employee = defaultEmployee;
  private addEmployeeSubscription?: Subscription
  private getEmployeeSubscription?: Subscription
  private updateEmployeeSubscription?: Subscription
  //endregion

  //region Constructor
  constructor(
    private toastService: ToastService,
    private datePipe: DatePipe,
    private employeeService: EmployeeService,
    private errorHandlingService: ErrorHandlerService
  ) {
  }

  //endregion

  //region Lifecycle Hooks
  /**
   * Initialize the component
   * If the employeeId is provided, load the employee details i.e. edit mode
   * If the employeeId is not provided, it is a new employee i.e. add mode
   * Convert the employment date to a valid date format
   */
  ngOnInit(): void {
    if (!this.employeeId) {
      this.title = 'Add Employee'
    } else {
      this.title = 'Edit Employee'
      this.mode = 'edit';
      this.getEmployeeById(this.employeeId.toString());
    }
    this.employee.employmentDate = inputDateFormat(this.employee.employmentDate, this.datePipe);
  }

  /**
   * Unsubscribe from the subscriptions
   */
  ngOnDestroy(): void {
    if (this.addEmployeeSubscription) {
      this.addEmployeeSubscription.unsubscribe()
    }
    if (this.getEmployeeSubscription) {
      this.getEmployeeSubscription.unsubscribe()
    }
    if (this.updateEmployeeSubscription) {
      this.updateEmployeeSubscription.unsubscribe()
    }
  }

//endregion

  //region Public Methods

  /**
   * Validate the selected date
   * - If the selected date is greater than today, mark the date as invalid
   * @param date - The date to validate
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

  /**
   * Handle the form submission
   * @param form - The form to submit
   */
  onFormSubmit(form: NgForm) {

    if (form.invalid) {
      this.message = 'Please correct the validation errors';
      return;
    }
    if (this.mode === 'add') {
      this.addEmployeeSubscription = this.employeeService.createEmployee(this.employee).subscribe({
        next: employee => {
          this.toastService.showSuccess(`Employee created with Id: ${employee.id}`)
          this.buttonLoading = false;
          this.employeeSaved.emit();
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
      this.updateEmployeeSubscription = this.employeeService.updateEmployee(this.employee).subscribe({
          next: employee => {
            this.toastService.showSuccess(`Employee saved`)
            this.buttonLoading = false;
            this.employeeSaved.emit();
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

  private getEmployeeById(id: string): void {
    this.getEmployeeSubscription = this.employeeService.getEmployee(id).subscribe({
      next: employee => {
        if (employee) {
          console.log(employee.id)
          employee.employmentDate = inputDateFormat(employee.employmentDate, this.datePipe);
          this.employee = employee;
        } else {
          this.toastService.showError('Employee not found')
        }
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandlingService.apiErrorHandler(err);
      }
    })
  }


}
