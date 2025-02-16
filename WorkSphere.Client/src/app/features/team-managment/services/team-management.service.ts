import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {Team} from '../model/team';

const API_URL = environment.apiUrl;

type GetTeamsResponse = {
  teams: Team[],
  totalRecords: number,
  pageSize: number,
  pageIndex: number
}


type updateTeamDto = {
  projectId: number,
  projectManagerId: number,
  teamMembers: Array<number>
}

@Injectable({
  providedIn: 'root'
})

export class TeamManagementService {

  constructor(
    private http: HttpClient
  ) {
  }

  getTeams(pageIndex: number, pageSize: number): Observable<GetTeamsResponse> {
    const apiMethod = `${API_URL}/api/teams?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<GetTeamsResponse>(apiMethod);
  }

  getTeam(id: number): Observable<Team> {
    const apiMethod = `${API_URL}/api/teams/${id}`;
    return this.http.get<Team>(apiMethod);
  }

  updateTeam(team: updateTeamDto): Observable<Team> {
    const apiMethod = `${API_URL}/api/teams`;
    return this.http.put<Team>(apiMethod, team);
  }
}
