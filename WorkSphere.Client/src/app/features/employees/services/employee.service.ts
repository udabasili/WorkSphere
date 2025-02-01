import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Employee} from '../model/employee';
import {environment} from '../../../../environments/environment';

const API_URL = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) {
  }

  /**
   * Get all employees
   * @param pageIndex The page index ie. the current page starting from 0
   * @param pageSize The page size ie. number of records per page
   * @returns An observable of the employees
   */
  getEmployees(pageIndex: number = 0, pageSize: number = 10): Observable<{
    employees: Employee[],
    pageIndex: number,
    pageSize: number,
    totalCount: number
  }> {
    const apiMethod = `${API_URL}/api/employees?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<{
      employees: Employee[],
      pageIndex: number,
      pageSize: number,
      totalCount: number
    }>(apiMethod);
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
}
