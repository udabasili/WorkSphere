import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {TeamManagementService} from '../../services/team-management.service';
import {Team} from '../../model/team';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {ConfirmationService, MenuItem} from 'primeng/api';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastService} from '../../../../services/toast.service';

@Component({
  selector: 'app-teams',
  standalone: false,

  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
export class TeamsComponent implements OnInit, OnDestroy {

  isLoading = true;
  selectedTeamId: number | null = null;
  visible: boolean = false;
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  header = "Teams";
  pageIndex = 0;
  pageSize = 10;
  teams: Array<Team> = []
  showDetailsTeam: boolean = false;
  totalRecords = 0;

  private deleteTeamSubscription?: Subscription;
  private getTeamSubscription?: Subscription;
  private routerSubscription?: Subscription;
  private getTeamsSubscription?: Subscription;

  constructor(
    private teamManagementService: TeamManagementService,
    private route: ActivatedRoute,
    private router: Router,
    private confirmationService: ConfirmationService,
    private toastService: ToastService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  ngOnInit(): void {
    this.handleRouteTeamSelection();

  }

  ngOnDestroy(): void {
    if (this.getTeamsSubscription) {
      this.getTeamsSubscription.unsubscribe();
    }
    if (this.deleteTeamSubscription) {
      this.deleteTeamSubscription.unsubscribe();
    }
    if (this.getTeamSubscription) {
      this.getTeamSubscription.unsubscribe();
    }
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
    this.showDetailsTeam = false;
    this.selectedTeamId = null;

  }

  handleTeamSelected(teamId: number) {
    this.router.navigate(['/team-management'], {queryParams: {id: teamId}});

  }

  prevPage() {

  }

  nextPage() {

  }

  handleClose() {
    this.visible = false;
  }

  private handleRouteTeamSelection(): void {
    this.routerSubscription = this.route.queryParams.subscribe({
      next: async (params) => {
        if (params['id']) {
          const teamId = parseInt(params['id'], 10);
          this.showDetailsTeam = true;
          this.selectedTeamId = teamId;
          this.items = [{label: 'Teams', url: 'team-management'}, {
            label: `${teamId}`,
            url: '/team-management',
            queryParams: {id: teamId}
          }]

        } else {
          await this.getTeams();
          this.showDetailsTeam = false
          this.items = [{label: 'Teams', url: '/team-management'}]
        }

      },
      error: (error) => {
        this.errorHandlerService.apiErrorHandler(error);
      }
    });
  }

  private getTeams(): void {
    this.getTeamsSubscription = this.teamManagementService.getTeams(0, 10).subscribe({
      next: (response) => {
        this.teams = response.teams;
        this.totalRecords = response.totalRecords;
        this.pageSize = response.pageSize;
        this.pageIndex = response.pageIndex
        this.isLoading = false;

      },
      error: (error) => {
        this.isLoading = false;
        this.errorHandlerService.apiErrorHandler(error);
      }
    })
  }
}
