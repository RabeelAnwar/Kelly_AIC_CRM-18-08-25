import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RightRequisitionSearchNavComponent } from './right-requisition-search-nav.component';

describe('RightRequisitionSearchNavComponent', () => {
  let component: RightRequisitionSearchNavComponent;
  let fixture: ComponentFixture<RightRequisitionSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RightRequisitionSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RightRequisitionSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
