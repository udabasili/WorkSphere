import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Subscription} from 'rxjs';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {ErrorHandlerService} from '../../../../core/services/error-handler.service';
import {Router} from '@angular/router';
import {ToastService} from '../../../../services/toast.service';

@Component({
  selector: 'app-project-details',
  standalone: false,
  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {
  @Input() visible: boolean = false;
  @Input() projectId: number | null = null;
  @Input() isLoading: boolean = false;
  @Output() closeLoader: EventEmitter<boolean> = new EventEmitter<boolean>();

  project: Project | null = null;
  projectSubscription?: Subscription

  constructor(
    private projectService: ProjectService,
    private errorHandlerService: ErrorHandlerService,
    private route: Router,
    private toastService: ToastService
  ) {
  }

  ngOnInit(): void {
    if (this.projectId) {
      this.loadProject(this.projectId);
    } else {
      this.toastService.showError('Project not found', 'error');
      this.route.navigate(['/projects']);
      this.isLoading = false;
    }
    this.handleLoaderCloser()
  }

  ngOnDestroy(): void {
    this.projectSubscription?.unsubscribe();
  }

  handleLoaderCloser() {
    this.closeLoader.emit(false);
    this.visible = false;
  }

  private loadProject(projectId: number) {
    this.projectSubscription = this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        if (!project) {
          this.toastService.showError('Project not found', 'error');
          this.route.navigate(['/projects']);
          this.isLoading = false;
          return;
        }
        this.project = project;
        this.visible = true;
        this.isLoading = false;
      },
      error: (err) => {
        this.visible = false;
        this.errorHandlerService.apiErrorHandler(err);
        this.isLoading = false;
      }
    });
  }
}
