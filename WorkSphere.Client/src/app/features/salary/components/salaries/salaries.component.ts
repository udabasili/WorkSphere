import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Salary} from '../../model/salary';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {Subject, Subscription, takeUntil} from 'rxjs';
import {ActivatedRoute, Router} from '@angular/router';
import {SalaryService} from '../../services/salary.service';
import {ToastService} from '../../../../services/toast.service';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

@Component({
  selector: 'app-salaries',
  standalone: false,

  templateUrl: './salaries.component.html',
  styleUrl: './salaries.component.css'
})
export class SalariesComponent implements OnInit, OnDestroy {
//region Properties
  @Input() visible = false;

  salaries: Salary[] = [];
  isLoading = true;
  showAddSalary = false;
  showDetailsSalary: boolean = false
  selectedSalaryId: number | null = null;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  header = "Salaries";
  pageIndex = 0;
  pageSize = 10;
  totalRecords = 0;

  private subscriptions: Subscription[] = [];
  //endregion

  //region Constructor
  constructor(
    private salaryService: SalaryService,
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
   *  - Handles the route salary selection
   *  - Subscribes to salary response
   *  - Fetches salaries
   *  - Handles the visibility change of the drawer
   */
  ngOnInit(): void {
    this.handleRouteSalarySelection();
    this.subscriptions.push(
      this.salaryService.salaryResponse$.subscribe((response) => {
        if (response) {
          this.salaries = response.salaries;
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
    this.showAddSalary = false;
    this.showDetailsSalary = false;
    this.selectedSalaryId = null;
  }

  //endregion

  //region Public Methods

  /**
   * Opens the drawer to add or edit an salary
   * @param salaryId - The salary id to edit. If not provided,
   * it will open the drawer to add a new salary
   */
  openDrawer(salaryId?: number) {
    if (salaryId) {
      this.selectedSalaryId = salaryId;
    } else {
      this.selectedSalaryId = null;
    }
    this.showAddSalary = true;
  }

  /**
   * Handles the pagination. Fetches the previous page of salaries
   * Only if the current page is not the first page
   */
  prevPage() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.getSalaries(this.pageIndex, this.pageSize);
    }

  }

  /**
   * Handles the pagination. Fetches the next page of salaries
   * Only if the current page is not the last page i.e.
   * the current page index * page size is less than the total records
   */
  nextPage() {
    if ((this.pageIndex + 1) * this.pageSize < this.totalRecords) {
      this.pageIndex++;
      this.getSalaries(this.pageIndex, this.pageSize);
    }
  }

  /**
   * Handles the visibility change of the drawer
   * @param isVisible - The visibility status of the drawer
   */
  handleDrawerVisibilityChange(isVisible: boolean) {
    this.showAddSalary = isVisible;
    if (!isVisible) {
      this.getSalaries(this.pageIndex, this.pageSize);
    }
  }

  /**
   * Fetches the salaries outside of behavior subject
   * @param pageIndex - The page index
   * @param pageSize - The page size
   */
  getSalaries(pageIndex: number = 0, pageSize: number): void {
    this.salaryService.refetchSalaries(pageIndex, pageSize);
  }

  /**
   * Handles the salary selection. Navigates to the salary details page
   * @param salary - The selected salary
   */
  handleSalarySelected(salary: Salary): void {
    this.router.navigate(['/salaries'], {queryParams: {id: salary.id}});
  }

  /**
   * Handles the salary deletion
   * @param event
   * @param salaryId
   */
  confirmDelete(event: Event, salaryId: number): void {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Do you want to delete this salary?',
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
        this.deleteSalary(salaryId);
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
  private handleRouteSalarySelection(): void {
    this.subscriptions.push(
      this.route.queryParams.pipe(takeUntil(new Subject())).subscribe({
        next: async (params) => {
          if (params['id']) {
            const salaryId = parseInt(params['id'], 10);
            this.selectedSalaryId = salaryId;
            this.showDetailsSalary = true;
            this.selectedSalaryId = salaryId;
            this.showDetailsSalary = true
            this.items = [{label: 'salaries', url: '/salaries'}, {
              label: `${salaryId}`,
              url: '/salaries',
              queryParams: {id: salaryId}
            }]

          } else {
            this.salaryService.getSalaries(this.pageIndex, this.pageSize);
            this.showDetailsSalary = false
            this.items = [{label: 'salaries', url: '/salaries'}]

          }
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

  private deleteSalary(salaryId: number): void {
    this.subscriptions.push(
      this.salaryService.deleteSalary(salaryId).subscribe({
        next: () => {
          this.toastService.showSuccess('Salary Deleted');
          this.pageIndex = 0;
          this.getSalaries(this.pageIndex, this.pageSize);
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      })
    );
  }

  //endregion
}
