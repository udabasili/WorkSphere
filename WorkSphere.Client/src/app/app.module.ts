import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {DashboardComponent} from './features/dashboard/components/dashboard.component';
import {HTTP_INTERCEPTORS, provideHttpClient} from '@angular/common/http';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {MatSlideToggle, MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {MatToolbar} from '@angular/material/toolbar';
import {MatListItem, MatNavList} from '@angular/material/list';
import {NavComponent} from './layout/nav/nav.component';
import {SideNavComponent} from './layout/side-nav/side-nav.component';
import {MatIcon} from '@angular/material/icon';
import {EmployeesComponent} from './features/employees/components/employees/employees.component';
import {MatFabButton, MatIconButton} from '@angular/material/button';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {EmployeeDetailsComponent} from './features/employees/components/employee-details/employee-details.component';
import {Drawer} from 'primeng/drawer';
import {Button, ButtonDirective} from 'primeng/button';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/aura';
import {MatBadge} from '@angular/material/badge';
import {HttpErrorInterceptor} from './core/interceptors/http-error.interceptor';
import {ActivityComponent} from './shared/components/activity/activity.component';
import {IftaLabel} from 'primeng/iftalabel';
import {InputText} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {Breadcrumb} from 'primeng/breadcrumb';
import {SectionHeaderComponent} from './shared/components/section-header/section-header.component';
import {ManageEmployeeComponent} from './features/employees/components/manage-employee/manage-employee.component';
import {DatePipe} from '@angular/common';
import {ConfirmationService, MessageService} from 'primeng/api';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {ProjectsComponent} from './features/project/components/projects/projects.component';
import {ProjectDetailsComponent} from './features/project/components/project-details/project-details.component';
import {ManageProjectComponent} from './features/project/components/manage-project/manage-project.component';
import {
  ProjectManagersComponent
} from './features/project-manager/components/project-managers/project-managers.component';
import {
  ManageProjectManagerComponent
} from './features/project-manager/components/manage-project-manager/manage-project-manager.component';
import {
  ProjectManagerDetailsComponent
} from './features/project-manager/components/project-manager-details/project-manager-details.component';
import {TasksComponent} from './features/task/components/tasks/tasks.component';
import {DropdownModule} from 'primeng/dropdown';
import {CdkDrag, CdkDropList} from '@angular/cdk/drag-drop';
import {TeamsComponent} from './features/team-managment/components/teams/teams.component';
import {TeamDetailsComponent} from './features/team-managment/components/team-details/team-details.component';
import {ManageTeamComponent} from './features/team-managment/components/manage-team/manage-team.component';
import {Dialog} from "primeng/dialog";
import {TextareaModule} from 'primeng/textarea';
import {SalaryDetailsComponent} from './features/salary/components/salary-details/salary-details.component';
import {ManageSalaryComponent} from './features/salary/components/manage-salary/manage-salary.component';
import {SalariesComponent} from './features/salary/components/salaries/salaries.component';
import {Skeleton} from "primeng/skeleton";
import {
  ProjectTaskChartComponent
} from './features/dashboard/components/project-task-chart/project-task-chart.component';
import {BaseChartDirective, provideCharts, withDefaultRegisterables} from 'ng2-charts';


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    NavComponent,
    SideNavComponent,
    EmployeesComponent,
    EmployeeDetailsComponent,
    ActivityComponent,
    SectionHeaderComponent,
    ManageEmployeeComponent,
    ProjectsComponent,
    ProjectDetailsComponent,
    ManageProjectComponent,
    ProjectManagersComponent,
    ManageProjectManagerComponent,
    ProjectManagerDetailsComponent,
    TasksComponent,
    TeamsComponent,
    TeamDetailsComponent,
    ManageTeamComponent,
    SalaryDetailsComponent,
    ManageSalaryComponent,
    SalariesComponent,
    ProjectTaskChartComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatSlideToggle,
    MatGridListModule,
    MatSidenavContent,
    MatToolbar,
    MatNavList,
    MatSidenavContainer,
    MatSidenav,
    MatListItem,
    MatIcon,
    MatFabButton,
    MatFormField,
    MatInput,
    MatIconButton,
    Drawer,
    Button,
    MatBadge,
    MatLabel,
    IftaLabel,
    InputText,
    FormsModule,
    Breadcrumb,
    DropdownModule,
    TextareaModule,
    BrowserAnimationsModule,  // Required for Toastr
    ToastrModule.forRoot({
      positionClass: 'toast-top-center',
      timeOut: 3000, // 3 seconds
      preventDuplicates: true,
    }),
    ConfirmDialogModule,
    CdkDropList,
    CdkDrag,
    Dialog,
    ButtonDirective,
    Skeleton,
    BaseChartDirective
  ],
  providers: [
    provideHttpClient(),
    provideAnimationsAsync(),
    MatSlideToggleModule,
    DatePipe,
    MessageService,
    ConfirmationService,
    provideCharts(withDefaultRegisterables()),
    {provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true},
    providePrimeNG({
      theme: {
        preset: Aura
      }
    })

  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
