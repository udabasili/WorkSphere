import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Project} from '../models/project';
import {environment} from '../../../../environments/environment';

const API_URL = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})

export class ProjectService {

  constructor(private httpClient: HttpClient) {
  }

  getProjects(pageIndex: number = 0, pageSize: number): Observable<{
    projects: Project[],
    pageIndex: number,
    pageSize: number,
    totalCount: number
  }> {
    const apiMethod = `${API_URL}/api/projects?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.httpClient.get<{
      projects: Project[],
      pageIndex: number,
      pageSize: number,
      totalCount: number
    }>(apiMethod)
  }

  getProject(id: number): Observable<Project> {
    const apiMethod = `${API_URL}/api/projects/${id}`;
    return this.httpClient.get<Project>(apiMethod)
  }

  creatProject(project: Project): Observable<Project> {
    const apiMethod = `${API_URL}/api/projects`;
    Reflect.deleteProperty(project, 'id');
    return this.httpClient.post<Project>(apiMethod, project)
  }

  updateProject(project: Project): Observable<Project> {
    const apiMethod = `${API_URL}/api/projects/${project.id}`;
    return this.httpClient.put<Project>(apiMethod, project)
  }

  deleteProject(id: number): Observable<boolean> {
    const apiMethod = `${API_URL}/api/projects/${id}`;
    return this.httpClient.delete<boolean>(apiMethod)
  }

}
