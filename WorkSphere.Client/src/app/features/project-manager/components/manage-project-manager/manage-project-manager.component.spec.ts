import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageProjectManagerComponent } from './manage-project-manager.component';

describe('ManageProjectManagerComponent', () => {
  let component: ManageProjectManagerComponent;
  let fixture: ComponentFixture<ManageProjectManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageProjectManagerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageProjectManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
