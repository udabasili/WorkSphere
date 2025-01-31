import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-activity',
  standalone: false,

  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent {
  @Input() message: string = 'Loading...';
  @Input() show: boolean = false;
}
