import {Component, Input, OnChanges, OnDestroy, OnInit} from '@angular/core';
import {firstValueFrom, Subject, Subscription, takeUntil} from 'rxjs';
import {EmployeeService} from '../../services/employee.service';
import {Employee} from '../../model/employee';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastService} from '../../../../services/toast.service';

@Component({
  selector: 'app-employees',
  standalone: false,
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})

export class EmployeesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() visible = false;

  employees: Employee[] = [];
  messages: string[] = [];
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
  private destroy$ = new Subject<void>();

  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
  ) {
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
    this.loadEmployees(this.pageIndex, this.pageSize);
  }

  nextPage() {
    this.pageIndex = this.pageIndex + 1;
    this.loadEmployees(this.pageIndex, this.pageSize);
  }

  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddEmployee = isVisible;
    if (!isVisible) {
      this.loadEmployees(
        this.pageIndex,
        this.pageSize
      ); // Reload employees when the drawer is closed
    }
  }

  ngOnInit(): void {
    this.loadEmployees(
      this.pageIndex,
      this.pageSize
    );
  }

  // CALL ON CHANGE WHEN A NEW EMPLOYEE IS ADDED i.e modal is closed
  ngOnChanges(): void {
    console.log('on changes')
  }

  async loadEmployees(pageIndex: number = 0, pageSize: number = 10) {
    try {
      //get all employees
      const response = await firstValueFrom(this.employeeService.getEmployees(pageIndex, pageSize));
      this.employees = response.employees;
      this.pageIndex = response.pageIndex;
      this.pageSize = response.pageSize;
      this.totalRecords = response.totalCount;
      this.isLoading = false;
      this.setSelectedEmployeeFromRoute();
    } catch (error: any) {
      this.messages.push(error.message || "Failed to load employees.");
      this.isLoading = false;
    }
  }

  setSelectedEmployeeFromRoute(): void {
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      const employeeId = parseInt(params['id'], 10);
      if (employeeId) {
        //show details
        const foundEmployee = this.employees.find(emp => emp.id === employeeId);
        if (foundEmployee) {
          this.selectedEmployeeId = foundEmployee.id;
          this.showDetailsEmployee = true
          this.items = [{label: 'Employees', url: '/employees'}, {
            label: `${employeeId}`,
            url: '/employees',
            queryParams: {id: employeeId}
          }]
        }
      } else {
        //show all employees
        this.showDetailsEmployee = false
        this.items = [{label: 'Employees', url: '/employees'}]
      }
    });
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
      reject: () => {
        this.toastService.showInfo('Delete Cancelled')
      },
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();

    if (this.deleteEmployeeSubscription) {
      this.deleteEmployeeSubscription.unsubscribe();
    }
  }

  private deleteEmployee(employeeId: number): void {
    this.deleteEmployeeSubscription = this.employeeService.deleteEmployee(employeeId).subscribe({
      next: () => {
        this.toastService.showSuccess('Employee Deleted');
        this.loadEmployees(this.pageIndex, this.pageSize);
      },
      error: (error) => {
        this.toastService.showError('Failed to delete employee');
      }
    });
  }
}
