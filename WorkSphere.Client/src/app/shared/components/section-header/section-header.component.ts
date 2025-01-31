import {Component, Input} from '@angular/core';
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'app-section-header',
  standalone: false,

  templateUrl: './section-header.component.html',
  styleUrl: './section-header.component.css'
})
export class SectionHeaderComponent {
  @Input() title: String = ""
  @Input() items: MenuItem[] | undefined;
  @Input() home: MenuItem | undefined;
}
