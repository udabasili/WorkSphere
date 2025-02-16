import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Subject, Subscription, takeUntil} from 'rxjs';
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
  //region Properties
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

  private subscriptions: Subscription[] = [];
  //endregion

  //region Constructor
  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  //endregion

  //region Lifecycle Hooks

  /**
   * Initializes the component
   *  - Handles the route employee selection
   *  - Subscribes to employee response
   *  - Fetches employees
   *  - Handles the visibility change of the drawer
   */
  ngOnInit(): void {
    this.handleRouteEmployeeSelection();
    this.subscriptions.push(
      this.employeeService.employeeResponse$.subscribe((response) => {
        if (response) {
          this.employees = response.employees;
          this.totalRecords = response.totalCount;
          this.pageIndex = response.pageIndex;
          this.pageSize = response.pageSize;
          this.isLoading = false;
        }
      })
    );
  }

  /**
   * Unsubscribes from all subscriptions
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    this.showAddEmployee = false;
    this.showDetailsEmployee = false;
    this.selectedEmployeeId = null;
  }

  //endregion

  //region Public Methods

  /**
   * Opens the drawer to add or edit an employee
   * @param employeeId - The employee id to edit. If not provided,
   * it will open the drawer to add a new employee
   */
  openDrawer(employeeId?: number) {
    if (employeeId) {
      this.selectedEmployeeId = employeeId;
    } else {
      this.selectedEmployeeId = null;
    }
    this.showAddEmployee = true;
  }

  /**
   * Handles the pagination. Fetches the previous page of employees
   * Only if the current page is not the first page
   */
  prevPage() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.getEmployees(this.pageIndex, this.pageSize);
    }

  }

  /**
   * Handles the pagination. Fetches the next page of employees
   * Only if the current page is not the last page i.e.
   * the current page index * page size is less than the total records
   */
  nextPage() {
    if ((this.pageIndex + 1) * this.pageSize < this.totalRecords) {
      this.pageIndex++;
      this.getEmployees(this.pageIndex, this.pageSize);
    }
  }

  /**
   * Handles the visibility change of the drawer
   * @param isVisible - The visibility status of the drawer
   */
  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddEmployee = isVisible;
    if (!isVisible) {
      this.getEmployees(this.pageIndex, this.pageSize);
    }
  }

  /**
   * Fetches the employees outside of behavior subject
   * @param pageIndex - The page index
   * @param pageSize - The page size
   */
  getEmployees(pageIndex: number = 0, pageSize: number): void {
    this.employeeService.refetchEmployees(pageIndex, pageSize);
  }

  /**
   * Handles the employee selection. Navigates to the employee details page
   * @param employee - The selected employee
   */
  handleEmployeeSelected(employee: Employee): void {
    this.router.navigate(['/employees'], {queryParams: {id: employee.id}});
  }

  /**
   * Handles the employee deletion
   * @param event
   * @param employeeId
   */
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

  //endregion

  //region Private Methods

  /**
   * Handles the route project selection
   * If a project id is present in the route, it will show the project details
   * Otherwise, it will show all projects
   * @private
   */
  private handleRouteEmployeeSelection(): void {
    this.subscriptions.push(
      this.route.queryParams.pipe(takeUntil(new Subject())).subscribe({
        next: async (params) => {
          if (params['id']) {
            const employeeId = parseInt(params['id'], 10);
            this.selectedEmployeeId = employeeId;
            this.showDetailsEmployee = true;
            this.selectedEmployeeId = employeeId;
            this.showDetailsEmployee = true
            this.items = [{label: 'Employees', url: '/employees'}, {
              label: `${employeeId}`,
              url: '/employees',
              queryParams: {id: employeeId}
            }]

          } else {
            this.getEmployees(this.pageIndex, this.pageSize);
            this.showDetailsEmployee = false
            this.items = [{label: 'Employees', url: '/employees'}]

          }
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

  private deleteEmployee(employeeId: number): void {
    this.subscriptions.push(
      this.employeeService.deleteEmployee(employeeId).subscribe({
        next: () => {
          this.toastService.showSuccess('Employee Deleted');
          this.pageIndex = 0;
          this.getEmployees(this.pageIndex, this.pageSize);
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

  //endregion

}
