import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTaskChartComponent } from './project-task-chart.component';

describe('ProjectTaskChartComponent', () => {
  let component: ProjectTaskChartComponent;
  let fixture: ComponentFixture<ProjectTaskChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectTaskChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectTaskChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
