import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {BehaviorSubject, Observable} from 'rxjs';
import {Team} from '../model/team';
import {ErrorHandlerService} from '../../../core/services/error-handler.service';

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

  private teamSubject = new BehaviorSubject<GetTeamsResponse | null>(null);
  teamResponse$ = this.teamSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorService: ErrorHandlerService
  ) {
  }

  getTeams(pageIndex: number = 0, pageSize: number): void {
    if (!this.teamSubject.value || this.teamSubject.value.pageIndex !== pageIndex) {
      this.fetchTeams(pageIndex, pageSize);
    }
  }

  refetchTeams(pageIndex: number = 0, pageSize: number): void {
    this.fetchTeams(pageIndex, pageSize);
  }

  getTeam(id: number): Observable<Team> {
    const apiMethod = `${API_URL}/api/teams/${id}`;
    return this.http.get<Team>(apiMethod);
  }

  updateTeam(team: updateTeamDto): Observable<Team> {
    const apiMethod = `${API_URL}/api/teams`;
    return this.http.put<Team>(apiMethod, team);
  }

  private fetchTeams(pageIndex: number, pageSize: number): void {
    const apiMethod = `${API_URL}/api/teams?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    this.http.get<GetTeamsResponse>(apiMethod).subscribe(
      response => {
        this.teamSubject.next(response);
      },
      error => {
        this.errorService.apiErrorHandler(error);
      }
    );
  }
}
