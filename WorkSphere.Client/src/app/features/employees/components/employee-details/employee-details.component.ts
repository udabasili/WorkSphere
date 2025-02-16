import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {EmployeeService} from '../../services/employee.service';
import {Employee} from '../../model/employee';
import {Subscription} from 'rxjs';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-employee-details',
  standalone: false,
  templateUrl: './employee-details.component.html',
  styleUrl: './employee-details.component.css'
})
export class EmployeeDetailsComponent implements OnInit, OnDestroy {
  //region Properties
  @Input() employeeId: number | null = null; // Accept employee data

  employee: Employee | null = null;
  employeeSubscription?: Subscription
  isLoading = true;
  //endregion

  //region Constructor
  constructor(
    private employeeService: EmployeeService,
    private errorHandlerService: ErrorHandlerService,
    private route: Router,
  ) {
  }

  //endregion

  //region Lifecycle Hooks
  ngOnInit(): void {
    if (this.employeeId) {
      this.loadEmployee(this.employeeId.toString());
    } else {
      this.isLoading = false;
      this.route.navigate(['/employees']);
    }
  }

  ngOnDestroy(): void {
    this.employeeSubscription?.unsubscribe();
  }

  //endregion

  //region Private Methods
  private loadEmployee(employeeId: string) {
    this.employeeSubscription = this.employeeService.getEmployee(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorHandlerService.apiErrorHandler(err);
        this.isLoading = false;
      }
    });
  }

  //endregion


}
