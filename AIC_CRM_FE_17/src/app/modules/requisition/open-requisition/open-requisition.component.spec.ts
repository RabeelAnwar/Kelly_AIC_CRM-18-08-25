import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpenRequisitionComponent } from './open-requisition.component';

describe('OpenRequisitionComponent', () => {
  let component: OpenRequisitionComponent;
  let fixture: ComponentFixture<OpenRequisitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OpenRequisitionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OpenRequisitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
