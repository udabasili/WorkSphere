import {Component, OnDestroy, OnInit} from '@angular/core';
import {EmployeeService} from '../../employees/services/employee.service';
import {Subscription} from 'rxjs';
import {ProjectService} from '../../project/services/project.service';
import {ProjectManagerService} from '../../project-manager/services/project-manager.service';
import {Team} from '../../team-managment/model/team';
import {TeamManagementService} from '../../team-managment/services/team-management.service';

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}


@Component({
  selector: 'app-dashboard',
  standalone: false,

  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  totalEmployees: number = 0;
  totalProjects: number = 0;
  totalProjectManagers: number = 0;
  teams: Team[] = [];

  private subscriptions: Subscription[] = [];

  constructor(
    private employeeService: EmployeeService,
    private projectService: ProjectService,
    private projectManagerService: ProjectManagerService,
    private teamService: TeamManagementService
  ) {

  }

  ngOnInit(): void {
    this.employeeService.getEmployees(0, 0)
    this.projectService.getProjects(0, 0)
    this.projectManagerService.getProjectManagers(0, 0)
    this.teamService.getTeams(0, 0)
    this.subscriptions.push(
      this.teamService.teamResponse$.subscribe((response) => {
        if (response) {
          this.teams = response.teams;
        }
      })
    )
    this.subscriptions.push(
      this.employeeService.employeeResponse$.subscribe((response) => {
        if (response) {
          this.totalEmployees = response.totalCount;
        }
      })
    )
    this.subscriptions.push(
      this.projectService.projectResponse$.subscribe((response) => {
        if (response) {
          this.totalProjects = response.totalCount;
        }
      })
    )
    this.subscriptions.push(
      this.projectManagerService.projectManagerResponse$.subscribe((response) => {
        if (response) {
          this.totalProjectManagers = response.totalCount;
        }
      })
    )
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
