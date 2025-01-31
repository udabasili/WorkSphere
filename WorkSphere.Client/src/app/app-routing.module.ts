import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {DashboardComponent} from './features/dashboard/components/dashboard.component';
import {EmployeesComponent} from './features/employees/components/employees/employees.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent
  },
  {
    path: 'employees',
    component: EmployeesComponent
  },
  { path: 'employees/:id', component: EmployeesComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
