import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-project-details',
  standalone: false,

  templateUrl: './project-details.component.html',
  styleUrl: './project-details.component.css'
})
export class ProjectDetailsComponent {
  @Input() projectId: number | null = null;
  @Input() visible: boolean = false;
}
