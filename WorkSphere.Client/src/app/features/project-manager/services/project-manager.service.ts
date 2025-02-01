import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../environments/environment';
import {ProjectManager} from '../model/project-manager';

const API_URL = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class ProjectManagerService {

  constructor(private http: HttpClient) {
  }

  /**
   * Get all projectManagers
   * @param pageIndex The page index ie. the current page starting from 0
   * @param pageSize The page size ie. number of records per page
   * @returns An observable of the projectManagers
   */
  getProjectManagers(pageIndex: number = 0, pageSize: number = 10): Observable<{
    projectManagers: ProjectManager[],
    pageIndex: number,
    pageSize: number,
    totalCount: number
  }> {
    const apiMethod = `${API_URL}/api/ProjectManagers?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<{
      projectManagers: ProjectManager[],
      pageIndex: number,
      pageSize: number,
      totalCount: number
    }>(apiMethod);
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
