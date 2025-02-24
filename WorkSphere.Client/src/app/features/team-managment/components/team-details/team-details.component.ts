import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Team} from '../../model/team';
import {Subscription} from 'rxjs';
import {TeamManagementService} from '../../services/team-management.service';
import {Router} from '@angular/router';
import {ProjectManager} from '../../../project-manager/model/project-manager';
import {Employee} from '../../../employees/model/employee';
import {ToastService} from '../../../../services/toast.service';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {ProjectManagerService} from '../../../project-manager/services/project-manager.service';
import {EmployeeService} from '../../../employees/services/employee.service';

@Component({
  selector: 'app-team-details',
  standalone: false,
  templateUrl: './team-details.component.html',
  styleUrls: ['./team-details.component.css']
})
export class TeamDetailsComponent implements OnDestroy, OnInit {

  //region Properties

  @Output() onClose: EventEmitter<void> = new EventEmitter<void>();
  @Input() visible!: boolean;
  @Input() teamId: number | null = null;

  team: Team | null = null;
  showProjectManagerDropdown: boolean = false;
  selectedNewProjectManager: number | null = null;
  availableProjectManagers: Array<ProjectManager> = [];
  showAddMemberDropdown: boolean = false;
  selectedNewMembers: Array<number> = [];
  availableTeamMembers: Array<Employee> = [];
  isLoading: boolean = false;
  isButtonLoading: boolean = false;

  teamResponse = {
    projectId: 0,
    projectManagerId: 0,
    teamMembers: Array<number>()
  };

  private subscriptions: Subscription[] = [];

  //endregion

  //region Constructor

  constructor(
    private teamService: TeamManagementService,
    private route: Router,
    private toastService: ToastService,
    private errorHandlingService: ErrorHandlerService,
    private projectManagerService: ProjectManagerService,
    private employeeService: EmployeeService,
    private projectService: ProjectManagerService
  ) {
  }

  //endregion

  //region Getter and Setter

  /**
   * Getter to retrieve the full name of the selected project manager.
   *
   * @returns {string} The full name of the project manager.
   */
  get projectManagerName(): string {
    const selectedManager = this.availableProjectManagers.find(manager =>
      Number(manager.id) === Number(this.selectedNewProjectManager)
    );
    return selectedManager ? selectedManager.fullName : this.team?.projectManager.fullName || '';
  }

  //endregion

  //region Lifecycle Hooks

  /**
   * Angular lifecycle hook that is called when the component is initialized.
   * Loads the team details and available project managers and employees.
   */
  ngOnInit(): void {
    if (this.teamId) {
      this.loadTeamData();
      this.loadAvailableProjectManagers();
      this.loadAvailableTeamMembers();

    } else {
      this.toastService.showError('Team not found', 'Error');
      this.route.navigate(['/team-management']);
    }

  }

  /**
   * Angular lifecycle hook that is called when the component is destroyed.
   * Unsubscribes from all active subscriptions.
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  //endregion

  //region Public Methods

  /**
   * Adds a member to the team by pushing the member's ID to the team members array.
   *
   * @param {number} id - The ID of the member to be added.
   */
  addTeamMember(id: number) {
    this.teamResponse.teamMembers.push(id);
  }

  /**
   * Toggles the visibility of the project manager dropdown
   * and resets the selected manager if closing.
   */
  toggleProjectManagerDropdown() {
    this.showProjectManagerDropdown = !this.showProjectManagerDropdown;
    if (!this.showProjectManagerDropdown) {
      this.selectedNewProjectManager = null;
    }
  }

  /**
   * Confirms the change of the project manager by setting the selected manager's ID.
   */
  confirmChangeProjectManager() {
    this.teamResponse.projectManagerId = Number(this.selectedNewProjectManager!);
    this.showProjectManagerDropdown = false;
  }

  /**
   * Toggles the visibility of the add member dropdown.
   * If closing, resets the selected members array.
   */
  toggleAddMemberDropdown() {
    this.showAddMemberDropdown = !this.showAddMemberDropdown;
    if (!this.showAddMemberDropdown) {
      this.selectedNewMembers = [];
    }
  }

  /**
   * Removes a member from the team by filtering out the member's ID
   * from the team members array.
   *
   * @param {number} id - The ID of the member to be removed.
   */
  removeTeamMember(id: number) {
    this.teamResponse.teamMembers = this.teamResponse.teamMembers.filter(memberId => memberId !== id);
  }

  /**
   * Confirms the selection of new team members and adds them to the team members array.
   */
  confirmTeamMemberSelection() {
    if (this.selectedNewMembers.length > 0) {
      this.selectedNewMembers.forEach(memberId => {
        this.teamResponse.teamMembers.push(memberId);
      });
    }
    this.showAddMemberDropdown = false;
  }

  /**
   * Updates the team details by calling the API with the updated team data.
   */
  updateTeam() {
    this.isButtonLoading = true;
    this.subscriptions.push(
      this.teamService.updateTeam(this.teamResponse).subscribe({
        next: () => {
          this.toastService.showSuccess('Team updated successfully', 'Success');
          this.isButtonLoading = false;
          this.route.navigate(['/team-management']);
        },
        error: (err) => {
          this.errorHandlingService.apiErrorHandler(err);
          this.isButtonLoading = false;
        }
      })
    );
  }

  //endregion

  //region Private Methods
  /**
   * Loads the team data from the API using the teamId.
   *
   * @private
   */
  private loadTeamData() {
    this.isLoading = true;
    this.subscriptions.push(
      this.teamService.getTeam(this.teamId!).subscribe({
        next: (team) => {
          if (!team) {
            this.handleError('Team not found');
            return;
          }
          this.team = team;
          this.teamResponse.projectId = team.id;
          this.teamResponse.projectManagerId = team.projectManager.id;
          this.teamResponse.teamMembers = team.teamMembers.map(member => member.id);
          this.isLoading = false;
          this.visible = true;
          this.filterOutCurrentTeamMembers();

        },
        error: (err) => {
          this.handleError(err);
        }
      })
    );
  }

  /**
   * Loads the available project managers from the API.
   *
   * @private
   */
  private loadAvailableProjectManagers() {
    this.projectManagerService.getProjectManagers(0, 30);
    this.subscriptions.push(
      this.projectManagerService.projectManagerResponse$.subscribe(response => {
        if (response) {
          this.availableProjectManagers = response.projectManagers;
        }
      })
    );
  }

  /**
   * Loads the available team members from the API.
   *
   * @private
   */
  private loadAvailableTeamMembers() {
    this.employeeService.getEmployees(0, 30);
    this.subscriptions.push(
      this.employeeService.employeeResponse$.subscribe(async response => {
        if (response) {
          this.availableTeamMembers = response.employees;
          this.filterOutCurrentTeamMembers();
        }
      })
    );
  }

  /**
   * Filters out current team members from the available employees list.
   *
   * @private
   */
  private filterOutCurrentTeamMembers() {
    if (this.team) {
      this.availableTeamMembers = this.availableTeamMembers.filter(employee =>
        !this.team.teamMembers.some(member => member.id === employee.id)
      );
    }
  }

  /**
   * Centralized error handler to display error messages and redirect.
   *
   * @param {string} message - The error message to be displayed.
   * @private
   */
  private handleError(message: string) {
    this.toastService.showError(message, 'error');
    this.route.navigate(['/teams']);
    this.isLoading = false;
    this.visible = false;
  }

  // #endregion

}
