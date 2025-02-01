import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectManagersComponent } from './project-managers.component';

describe('ProjectManagersComponent', () => {
  let component: ProjectManagersComponent;
  let fixture: ComponentFixture<ProjectManagersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectManagersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectManagersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
