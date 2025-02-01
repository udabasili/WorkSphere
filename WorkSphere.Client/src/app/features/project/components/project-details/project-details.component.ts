import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Subscription} from 'rxjs';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';

@Component({
  selector: 'app-project-details',
  standalone: false,

  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {
  @Input() visible: boolean = false;
  @Input() projectId: number | null = null; // Accept project data
  @Input() isLoading: boolean = false;

  @Output() closeLoader: EventEmitter<boolean> = new EventEmitter<boolean>();

  project: Project | null = null;
  projectSubscription?: Subscription

  constructor(private projectService: ProjectService) {
  }

  ngOnInit(): void {
    if (this.projectId) {
      this.loadProject(this.projectId);
    } else {
      console.error('Project ID is required');
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
        // if (!project.projectTasks || project.projectTasks.length === 0) {
        //   project.projectTasks = [];
        // }
        this.project = project;
        this.visible = true;
      },
      error: (err) => {
        console.error(err);
        this.visible = false;
      }
    });
  }
}
