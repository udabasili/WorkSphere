import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';
import {ProjectManager} from '../../model/project-manager';
import {ProjectManagerService} from '../../services/project-manager.service';
import {Team} from '../../../team-managment/model/team';

@Component({
  selector: 'app-project-manager-details',
  standalone: false,

  templateUrl: './project-manager-details.component.html',
  styleUrl: './project-manager-details.component.css'
})
export class ProjectManagerDetailsComponent implements OnDestroy, OnInit {
  @Input() projectManagerId: number | null = null; // Accept projectManager data

  projectManager: ProjectManager | null = null;
  projectManagerSubscription?: Subscription
  selectedTeam: Team | null = null;

  constructor(private projectManagerService: ProjectManagerService) {
  }

  ngOnInit(): void {
    if (this.projectManagerId) {
      this.loadProjectManager(this.projectManagerId.toString());
    } else {
      console.error('ProjectManager ID is required');
    }
  }

  ngOnDestroy(): void {
    this.projectManagerSubscription?.unsubscribe();

  }


  private loadProjectManager(projectManagerId: string) {
    this.projectManagerSubscription = this.projectManagerService.getProjectManager(projectManagerId).subscribe({
      next: (projectManager) => {
        this.projectManager = projectManager;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
