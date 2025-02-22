import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DashboardComponent} from './features/dashboard/components/dashboard.component';
import {EmployeesComponent} from './features/employees/components/employees/employees.component';
import {ProjectsComponent} from './features/project/components/projects/projects.component';
import {
  ProjectManagersComponent
} from './features/project-manager/components/project-managers/project-managers.component';
import {TasksComponent} from './features/task/components/tasks/tasks.component';
import {TeamsComponent} from './features/team-managment/components/teams/teams.component';
import {SalariesComponent} from './features/salary/components/salaries/salaries.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent
  },
  //Employees route
  {
    path: 'employees',
    component: EmployeesComponent
  },
  {path: 'employees/:id', component: EmployeesComponent},
  //Projects route
  {
    path: 'projects',
    component: ProjectsComponent
  },
  {
    path: 'projects/:id',
    component: ProjectsComponent
  },
  //Project Managers route
  {
    path: 'project-managers',
    component: ProjectManagersComponent
  },
  {
    path: 'project-managers/:id',
    component: ProjectManagersComponent
  },
  //Tasks route
  {
    path: 'tasks',
    component: TasksComponent
  },
  //team-management route
  {
    path: 'team-management',
    component: TeamsComponent
  },
  {
    path: 'team-management/:id',
    component: TeamsComponent
  },
  //salary route
  {
    path: 'salaries',
    component: SalariesComponent
  },
  {
    path: 'salaries/:id',
    component: SalariesComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
