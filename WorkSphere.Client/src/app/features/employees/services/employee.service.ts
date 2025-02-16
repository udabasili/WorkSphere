import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {Employee} from '../model/employee';
import {environment} from '../../../../environments/environment';
import {ErrorHandlerService} from '../../../core/services/error-handler.service';
import {catchError} from 'rxjs/operators';

const API_URL = environment.apiUrl;

export type EmployeeResponse = {
  employees: Employee[],
  pageIndex: number,
  pageSize: number,
  totalCount: number
}

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  private employeeSubject = new BehaviorSubject<EmployeeResponse | null>(null);
  employeeResponse$ = this.employeeSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorService: ErrorHandlerService
  ) {
  }

  // Get all employees with behavior subject
  getEmployees(pageIndex: number = 0, pageSize: number): void {
    if (!this.employeeSubject.value || this.employeeSubject.value.pageIndex !== pageIndex) {
      this.fetchEmployees(pageIndex, pageSize);
    }
  }

  // Refetch employees without condition
  refetchEmployees(pageIndex: number = 0, pageSize: number): void {
    this.fetchEmployees(pageIndex, pageSize);
  }

  /**
   *
   * @param id
   * @returns An observable of the employee
   */
  getEmployee(id: string): Observable<Employee> {
    const apiMethod = `${API_URL}/api/employees/${id}`;
    return this.http.get<Employee>(apiMethod)

  }

  /**
   *
   * @param employee
   */
  createEmployee(employee: Employee): Observable<Employee> {
    const apiMethod = `${API_URL}/api/employees`;
    //remove the id property from the employee object
    Reflect.deleteProperty(employee, 'id');
    return this.http.post<Employee>(apiMethod, employee);
  }

  updateEmployee(employee: Employee): Observable<Employee> {
    const apiMethod = `${API_URL}/api/employees/${employee.id}`;
    return this.http.put<Employee>(apiMethod, employee);
  }

  deleteEmployee(id: number): Observable<boolean> {
    const apiMethod = `${API_URL}/api/employees/${id}`;
    return this.http.delete<boolean>(apiMethod);
  }

  private fetchEmployees(pageIndex: number, pageSize: number): void {
    const apiMethod = `${API_URL}/api/employees?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    this.http
      .get<EmployeeResponse>(apiMethod)
      .pipe(
        catchError((error) => {
          this.errorService.apiErrorHandler(error);
          return of(null);
        })
      )
      .subscribe((response) => {
        this.employeeSubject.next(response);
      });
  }
}
