import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {EmployeeService} from '../../services/employee.service';
import {Employee} from '../../model/employee';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-employee-details',
  standalone: false,
  templateUrl: './employee-details.component.html',
  styleUrl: './employee-details.component.css'
})
export class EmployeeDetailsComponent implements OnInit, OnDestroy {
  @Input() employeeId: number | null = null; // Accept employee data

  employee: Employee | null = null;
  employeeSubscription?: Subscription

  constructor(private employeeService: EmployeeService) {
  }

  ngOnInit(): void {
    if (this.employeeId) {
      this.loadEmployee(this.employeeId.toString());
    } else {
      console.error('Employee ID is required');
    }
  }

  ngOnDestroy(): void {
    this.employeeSubscription?.unsubscribe();
  }

  private loadEmployee(employeeId: string) {
    this.employeeSubscription = this.employeeService.getEmployee(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }


}
