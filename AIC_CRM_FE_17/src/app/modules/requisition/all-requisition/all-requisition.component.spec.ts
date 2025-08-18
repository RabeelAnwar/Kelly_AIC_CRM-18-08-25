import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllRequisitionComponent } from './all-requisition.component';

describe('AllRequisitionComponent', () => {
  let component: AllRequisitionComponent;
  let fixture: ComponentFixture<AllRequisitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllRequisitionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllRequisitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
