import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable, of} from 'rxjs';
import {environment} from '../../../../environments/environment';
import {ProjectManager} from '../model/project-manager';
import {ErrorHandlerService} from '../../../core/services/error-handler.service';
import {catchError} from 'rxjs/operators';

const API_URL = environment.apiUrl;

type ProjectManagerResponse = {
  projectManagers: ProjectManager[],
  pageIndex: number,
  pageSize: number,
  totalCount: number
}

@Injectable({
  providedIn: 'root'
})

export class ProjectManagerService {

  private projectManagerSubject = new BehaviorSubject<ProjectManagerResponse | null>(null);
  projectManagerResponse$ = this.projectManagerSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorService: ErrorHandlerService
  ) {
  }

  /**
   * Get all projectManagers
   * @param pageIndex The page index ie. the current page starting from 0
   * @param pageSize The page size ie. number of records per page
   * @returns An observable of the projectManagers
   */
  getProjectManagers(pageIndex: number = 0, pageSize: number = 10): void {
    if (!this.projectManagerSubject.value || this.projectManagerSubject.value.pageIndex !== pageIndex) {
      this.fetchProjectManagers(pageIndex, pageSize);
    }
  }

  refetchProjectManagers(pageIndex: number, pageSize: number) {
    this.fetchProjectManagers(pageIndex, pageSize);
  }

  fetchProjectManagers(pageIndex: number, pageSize: number) {
    const apiMethod = `${API_URL}/api/ProjectManagers?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    this.http.get<{
      projectManagers: ProjectManager[],
      pageIndex: number,
      pageSize: number,
      totalCount: number
    }>(apiMethod)
      .pipe(catchError(error => {
        this.errorService.apiErrorHandler(error);
        return of(null); // Return a fallback value or handle the error as needed
      }))
      .subscribe(response => {
        this.projectManagerSubject.next(response)
      })
  }

  /**
   *
   * @param id
   * @returns An observable of the projectManager
   */
  getProjectManager(id: string): Observable<ProjectManager> {
    const apiMethod = `${API_URL}/api/ProjectManagers/${id}`;
    return this.http.get<ProjectManager>(apiMethod)

  }

  /**
   *
   * @param projectManager
   */
  createProjectManager(projectManager: ProjectManager): Observable<ProjectManager> {
    const apiMethod = `${API_URL}/api/projectManagers`;
    //remove the id property from the projectManager object
    Reflect.deleteProperty(projectManager, 'id');
    return this.http.post<ProjectManager>(apiMethod, projectManager);
  }

  updateProjectManager(projectManager: ProjectManager): Observable<ProjectManager> {
    const apiMethod = `${API_URL}/api/ProjectManagers/${projectManager.id}`;
    return this.http.put<ProjectManager>(apiMethod, projectManager);
  }

  deleteProjectManager(id: number): Observable<boolean> {
    const apiMethod = `${API_URL}/api/ProjectManagers/${id}`;
    return this.http.delete<boolean>(apiMethod);
  }
}
