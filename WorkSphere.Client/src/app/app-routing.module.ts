import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DashboardComponent} from './features/dashboard/components/dashboard.component';
import {EmployeesComponent} from './features/employees/components/employees/employees.component';
import {ProjectsComponent} from './features/project/components/projects/projects.component';

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
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
