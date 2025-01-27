import { Injectable } from '@angular/core';
import {API_URL, SharedService} from './shared.service';
import {HttpClient} from '@angular/common/http';
import {catchError, Observable} from 'rxjs';
import {Employee} from '../models/employee';


@Injectable({
  providedIn: 'root'
})
export class EmployeeService extends SharedService {

  constructor(private  http: HttpClient) {
    super();
  }

  getEmployees():Observable<Employee> {
    const apiMethod = `${API_URL}/api/employees`;
    return  this.http.get<Employee>(apiMethod).pipe(catchError(super.handleError));

  }
}
