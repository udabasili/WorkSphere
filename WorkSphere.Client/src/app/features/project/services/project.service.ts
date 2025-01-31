import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Project} from '../models/project';
import {API_URL} from '../../../services/shared.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private httpClient: HttpClient) {
  }

  getProjects(pageIndex: number = 0, pageSize: number): Observable<Array<Project>> {
    const apiMethod = `${API_URL}/api/projects?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.httpClient.get<Array<Project>>(apiMethod)
  }

  getProject(id: number): Observable<Project> {
    const apiMethod = `${API_URL}/api/projects?id=${id}`;
    return this.httpClient.get<Project>(apiMethod)
  }

  creatProject(project: Project): Observable<Project> {
    const apiMethod = `${API_URL}/api/projects}`;
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
