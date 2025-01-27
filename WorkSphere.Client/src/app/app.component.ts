import {Component, OnDestroy, OnInit} from '@angular/core';
import {EmployeeService} from './services/employee.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'WorkSphere.Client';

  employeeSub?: Subscription;

  constructor(private employeeService:EmployeeService) {
  }

  ngOnInit(): void {
    this.employeeSub = this.employeeService.getEmployees().subscribe({
      next: employees => {
        console.log(employees);
      },
      error: err => {
        console.log(err);
      }
    });

  }

  ngOnDestroy(): void {
    if(this.employeeSub){
      this.employeeSub.unsubscribe();
    }
  }
}
