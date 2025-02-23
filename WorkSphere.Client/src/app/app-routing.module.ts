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
import {LoginComponent} from './features/auth/components/login/login.component';
import {AuthGuard} from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard]

  },
  //Employees route
  {
    path: 'employees',
    component: EmployeesComponent,
    // canActivate: [AuthGuard]
  },
  {
    path: 'employees/:id',
    component: EmployeesComponent,
    canActivate: [AuthGuard]
  },
  //Projects route
  {
    path: 'projects',
    component: ProjectsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'projects/:id',
    component: ProjectsComponent,
    canActivate: [AuthGuard]
  },
  //Project Managers route
  {
    path: 'project-managers',
    component: ProjectManagersComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'project-managers/:id',
    component: ProjectManagersComponent,
    canActivate: [AuthGuard]
  },
  //Tasks route
  {
    path: 'tasks',
    component: TasksComponent,
    canActivate: [AuthGuard]
  },
  //team-management route
  {
    path: 'team-management',
    component: TeamsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'team-management/:id',
    component: TeamsComponent,
    canActivate: [AuthGuard]
  },
  //salary route
  {
    path: 'salaries',
    component: SalariesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'salaries/:id',
    component: SalariesComponent,
    canActivate: [AuthGuard]
  },
  //Login route
  {
    path: 'auth/login',
    component: LoginComponent,
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
