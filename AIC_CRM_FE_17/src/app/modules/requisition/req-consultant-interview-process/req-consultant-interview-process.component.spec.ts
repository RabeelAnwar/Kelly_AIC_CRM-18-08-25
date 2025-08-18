import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReqConsultantInterviewProcessComponent } from './req-consultant-interview-process.component';

describe('ReqConsultantInterviewProcessComponent', () => {
  let component: ReqConsultantInterviewProcessComponent;
  let fixture: ComponentFixture<ReqConsultantInterviewProcessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReqConsultantInterviewProcessComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReqConsultantInterviewProcessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
