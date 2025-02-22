import {Injectable} from '@angular/core';
import {ErrorHandlerService} from '../../../core/services/error-handler.service';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Salary} from '../model/salary';
import {environment} from '../../../../environments/environment';

const API_URL = environment.apiUrl;

type SalaryResponse = {
  salaries: Salary[],
  pageIndex: number,
  pageSize: number,
  totalCount: number
}

@Injectable({
  providedIn: 'root'
})
export class SalaryService {
  private salarySubject = new BehaviorSubject<SalaryResponse | null>(null);
  salaryResponse$ = this.salarySubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorService: ErrorHandlerService
  ) {
  }

  // Get all salaries with behavior subject
  getSalaries(pageIndex: number = 0, pageSize: number): void {
    if (!this.salarySubject.value || this.salarySubject.value.pageIndex !== pageIndex) {
      this.fetchSalaries(pageIndex, pageSize);
    }
  }

  // Refetch salaries without condition
  refetchSalaries(pageIndex: number = 0, pageSize: number): void {
    this.fetchSalaries(pageIndex, pageSize);
  }

  /**
   *
   * @param id
   * @returns An observable of the salary
   */
  getSalary(id: string): Observable<Salary> {
    const apiMethod = `${API_URL}/api/salaries/${id}`;
    return this.http.get<Salary>(apiMethod)
  }

  /**
   *
   * @param salary
   */
  createSalary(salary: Salary): Observable<Salary> {
    const apiMethod = `${API_URL}/api/salaries`;
    return this.http.post<Salary>(apiMethod, salary)
  }

  /**
   *
   * @param salary
   */
  updateSalary(salary: Salary): Observable<Salary> {
    const apiMethod = `${API_URL}/api/salaries/${salary.id}`;
    return this.http.put<Salary>(apiMethod, salary)
  }

  deleteSalary(id: number): Observable<boolean> {
    const apiMethod = `${API_URL}/api/salaries/${id}`;
    return this.http.delete<boolean>(apiMethod);
  }

  private fetchSalaries(pageIndex: number, pageSize: number): void {
    const apiMethod = `${API_URL}/api/salaries?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    this.http.get<SalaryResponse>(apiMethod).subscribe({
      next: (response) => {
        this.salarySubject.next(response);
      },
      error: (err) => {
        this.errorService.apiErrorHandler(err);
      }
    });
  }

}
