import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Subscription} from 'rxjs';
import {ToastService} from '../../../../services/toast.service';
import {DatePipe} from '@angular/common';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {NgForm} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';
import {Salary} from '../../model/salary';
import {SalaryService} from '../../services/salary.service';

const defaultSalary: Salary = {
  id: null,
  employeeID: null,
  projectManagerID: null,
  basicSalary: 0,
  deductions: 0,
  bonus: 0,
  totalSalary: 0,
}

@Component({
  selector: 'app-manage-salary',
  standalone: false,

  templateUrl: './manage-salary.component.html',
  styleUrl: './manage-salary.component.css'
})
export class ManageSalaryComponent implements OnInit, OnDestroy {
  //region Properties
  @Input() visible: boolean = false;
  @Input() salaryId: number | null = null;
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() salarySaved: EventEmitter<null> = new EventEmitter<null>();

  title: String = ''
  message: string = '';
  saveMessage: string = '';
  mode: string = 'add';
  buttonLoading: boolean = false;
  salary: Salary = defaultSalary;
  private getSalarySubscription?: Subscription
  private updateSalarySubscription?: Subscription
  //endregion

  //region Constructor
  constructor(
    private toastService: ToastService,
    private datePipe: DatePipe,
    private salaryService: SalaryService,
    private errorHandlingService: ErrorHandlerService
  ) {
  }

  //endregion

  //region Lifecycle Hooks
  /**
   * Initialize the component
   * If the salaryId is provided, load the salary details i.e. edit mode
   * If the salaryId is not provided, it is a new salary i.e. add mode
   * Convert the employment date to a valid date format
   */
  ngOnInit(): void {
    if (!this.salaryId) {
      this.title = 'Add Salary'
    } else {
      this.title = 'Edit Salary'
      this.mode = 'edit';
      this.getSalaryById(this.salaryId.toString());
    }
  }

  /**
   * Unsubscribe from the subscriptions
   */
  ngOnDestroy(): void {

    if (this.getSalarySubscription) {
      this.getSalarySubscription.unsubscribe()
    }
    if (this.updateSalarySubscription) {
      this.updateSalarySubscription.unsubscribe()
    }
  }

//endregion

  //region Public Methods

 
  /**
   * Handle the form submission
   * @param form - The form to submit
   */
  onFormSubmit(form: NgForm) {

    if (form.invalid) {
      this.message = 'Please correct the validation errors';
      return;
    }

    this.updateSalarySubscription = this.salaryService.updateSalary(this.salary).subscribe({
        next: salary => {
          this.toastService.showSuccess(`Salary saved`)
          this.buttonLoading = false;
          this.salarySaved.emit();
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


  onClose() {
    this.visible = false;
    this.visibilityChange.emit(false);
  }

  private getSalaryById(id: string): void {
    this.getSalarySubscription = this.salaryService.getSalary(id).subscribe({
      next: salary => {
        if (salary) {
          this.salary = salary;
        } else {
          this.toastService.showError('Salary not found')
        }
      },
      error: (err: HttpErrorResponse) => {
        this.errorHandlingService.apiErrorHandler(err);
      }
    })
  }


}
