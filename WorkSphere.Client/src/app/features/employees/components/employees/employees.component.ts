import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {firstValueFrom, Subject, Subscription, takeUntil} from 'rxjs';
import {EmployeeService} from '../../services/employee.service';
import {Employee} from '../../model/employee';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastService} from '../../../../services/toast.service';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

@Component({
  selector: 'app-employees',
  standalone: false,
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})

export class EmployeesComponent implements OnInit, OnDestroy {
  @Input() visible = false;

  employees: Employee[] = [];
  isLoading = true;
  showAddEmployee = false;
  showDetailsEmployee: boolean = false
  selectedEmployeeId: number | null = null;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  header = "Employees";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;

  private deleteEmployeeSubscription?: Subscription;
  private routeDestroy$ = new Subject<void>();

  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  ngOnInit(): void {
    this.handleRouteEmployeeSelection();
  }

  ngOnDestroy(): void {
    this.routeDestroy$.next(); // Unsubscribe from all observables
    this.routeDestroy$.complete(); // Complete the subject
    if (this.deleteEmployeeSubscription) {
      this.deleteEmployeeSubscription.unsubscribe();
    }
    this.showAddEmployee = false;
    this.showDetailsEmployee = false;
    this.selectedEmployeeId = null;

  }

  openDrawer(employeeId?: number) {
    if (employeeId) {
      this.selectedEmployeeId = employeeId;
    } else {
      this.selectedEmployeeId = null;
    }
    this.showAddEmployee = true;
  }

  prevPage() {
    this.pageIndex = this.pageIndex - 1;
    this.loadEmployees();
  }

  nextPage() {
    this.pageIndex = this.pageIndex + 1;
    this.loadEmployees();
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddEmployee = isVisible;
    if (!isVisible) {
      this.loadEmployees(

      ); // Reload employees when the drawer is closed
    }
  }

  async loadEmployees() {
    try {
      //get all employees
      //firstValueFrom is used to convert an observable to a promise
      const response = await firstValueFrom(this.employeeService.getEmployees(this.pageIndex, this.pageSize));
      this.employees = response.employees;
      this.pageIndex = response.pageIndex;
      this.pageSize = response.pageSize;
      this.totalRecords = response.totalCount;
      this.isLoading = false;
    } catch (error: any) {
      this.errorHandlerService.apiErrorHandler(error);
      this.isLoading = false;

    }
  }

  handleEmployeeSelected(employee: Employee): void {
    this.router.navigate(['/employees'], {queryParams: {id: employee.id}});
  }

  confirmDelete(event: Event, employeeId: number): void {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this employee?',
      header: 'Delete Confirmation',
      icon: 'pi pi-info-circle',
      rejectLabel: 'Cancel',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },
      accept: () => {
        this.deleteEmployee(employeeId);
      },
    });
  }


  /**
   * Handles the route project selection
   * If a project id is present in the route, it will show the project details
   * Otherwise, it will show all projects
   * @private
   */
  private handleRouteEmployeeSelection(): void {
    this.route.queryParams.pipe(takeUntil(this.routeDestroy$)).subscribe(params => {
      const employeeId = parseInt(params['id'], 10);
      if (employeeId) {
        this.selectedEmployeeId = employeeId;
        this.showDetailsEmployee = true
        this.items = [{label: 'Employees', url: '/employees'}, {
          label: `${employeeId}`,
          url: '/employees',
          queryParams: {id: employeeId}
        }]

      } else {
        this.loadEmployees();
        this.showDetailsEmployee = false
        this.items = [{label: 'Employees', url: '/employees'}]

      }
    });
  }

  private deleteEmployee(employeeId: number): void {
    this.deleteEmployeeSubscription = this.employeeService.deleteEmployee(employeeId).subscribe({
      next: () => {
        this.toastService.showSuccess('Employee Deleted');
        this.pageIndex = 0;
        this.loadEmployees();
      },
      error: (error) => {
        this.errorHandlerService.apiErrorHandler(error);
      }
    });
  }

}
