import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftRequisitionSearchNavComponent } from './left-requisition-search-nav.component';

describe('LeftRequisitionSearchNavComponent', () => {
  let component: LeftRequisitionSearchNavComponent;
  let fixture: ComponentFixture<LeftRequisitionSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftRequisitionSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftRequisitionSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
