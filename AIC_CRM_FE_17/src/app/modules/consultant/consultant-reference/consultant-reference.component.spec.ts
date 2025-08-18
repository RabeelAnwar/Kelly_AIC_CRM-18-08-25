import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultantReferenceComponent } from './consultant-reference.component';

describe('ConsultantReferenceComponent', () => {
  let component: ConsultantReferenceComponent;
  let fixture: ComponentFixture<ConsultantReferenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultantReferenceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultantReferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
