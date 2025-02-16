import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {Employee} from '../../employees/model/employee';
import {Task} from '../model/task';

const API_URL = environment.apiUrl;

export type TasksAPIResponse = {
  projectTasks: Task[]
  projectTeamMembers: Employee[]
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private httpClient: HttpClient) {
  }

  getTasks(projectId: number): Observable<TasksAPIResponse> {
    const apiMethod = `${API_URL}/api/ProjectTasks?projectID=${projectId}`;
    return this.httpClient.get<TasksAPIResponse>(apiMethod);
  }

  updateTasks(tasks: Array<Task>, projectId: number) {
    const apiMethod = `${API_URL}/api/ProjectTasks?projectID=${projectId}`;
    return this.httpClient.put<void>(apiMethod, {tasks});
  }

}
