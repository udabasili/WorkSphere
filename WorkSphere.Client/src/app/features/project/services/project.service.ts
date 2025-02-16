import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {Project} from '../models/project';
import {environment} from '../../../../environments/environment';

const API_URL = environment.apiUrl;
export type ProjectResponse = {
  projects: Project[],
  pageIndex: number,
  pageSize: number,
  totalCount: number
}

@Injectable({
  providedIn: 'root'
})

export class ProjectService {
  private projectSubject = new BehaviorSubject<ProjectResponse | null>(null);
  projectResponse$ = this.projectSubject.asObservable();

  constructor(private httpClient: HttpClient) {
  }

  getProjects(pageIndex: number = 0, pageSize: number): void {
    if (!this.projectSubject.value || this.projectSubject.value.pageIndex !== pageIndex) {
      this.fetchProjects(pageIndex, pageSize);
    }
  }

  refreshProjects(pageIndex: number, pageSize: number): void {
    this.fetchProjects(pageIndex, pageSize);
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

  private fetchProjects(pageIndex: number, pageSize: number) {
    const apiMethod = `${API_URL}/api/projects?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    this.httpClient.get<{
      projects: Project[],
      pageIndex: number,
      pageSize: number,
      totalCount: number
    }>(apiMethod).subscribe(response => {
      this.projectSubject.next(response)
    })
  }

}
