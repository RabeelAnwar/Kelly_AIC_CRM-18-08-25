import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultantActivityComponent } from './consultant-activity.component';

describe('ConsultantActivityComponent', () => {
  let component: ConsultantActivityComponent;
  let fixture: ComponentFixture<ConsultantActivityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultantActivityComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultantActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
