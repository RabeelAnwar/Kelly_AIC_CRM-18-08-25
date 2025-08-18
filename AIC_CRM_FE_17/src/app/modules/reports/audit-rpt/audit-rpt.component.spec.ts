import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditRptComponent } from './audit-rpt.component';

describe('AuditRptComponent', () => {
  let component: AuditRptComponent;
  let fixture: ComponentFixture<AuditRptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditRptComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuditRptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
