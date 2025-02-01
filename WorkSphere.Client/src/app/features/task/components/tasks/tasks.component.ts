import {Component, OnDestroy, OnInit} from '@angular/core';
import {ConfirmationService, MenuItem, MessageService} from 'primeng/api';
import {ProjectService} from '../../../project/services/project.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';

interface ProjectOption {
  id: number;
  name: string;
}

@Component({
  selector: 'app-tasks',
  standalone: false,
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.css'
})
export class TasksComponent implements OnInit, OnDestroy {
  items: MenuItem[] = [];
  home: MenuItem = {label: 'Home', url: '/'};
  readonly header = "Tasks";
  projectSelectionOptions: ProjectOption[] = [];
  selectedProject: ProjectOption | undefined

  getProjectsSubscription?: Subscription
  teamMembers: any[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private projectService: ProjectService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private errorHandlerService: ErrorHandlerService,
  ) {
  }

  ngOnInit() {
    this.getProjects();
  }

  ngOnDestroy() {
    this.getProjectsSubscription?.unsubscribe();
  }

  getProjects() {
    this.getProjectsSubscription = this.projectService.getProjects(0, 30)
      .subscribe({
        next: (apiResponse) => {
          const projects = apiResponse.projects;
          this.projectSelectionOptions = projects.map((project) => {
            return {id: project.id, name: project.name};
          });
        },
        error: (error) => {
          this.errorHandlerService.apiErrorHandler(error);
        }
      });
  }

  onProjectSelected() {

  }

  addTeam() {
    
  }
}
