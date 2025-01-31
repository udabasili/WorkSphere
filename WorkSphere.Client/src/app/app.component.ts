import {Component, OnDestroy, OnInit} from '@angular/core';
import {EmployeeService} from './features/employees/services/employee.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent  {
  title = 'WorkSphere.Client';


}
