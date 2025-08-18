import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultantTopNavComponent } from './consultant-top-nav.component';

describe('ConsultantTopNavComponent', () => {
  let component: ConsultantTopNavComponent;
  let fixture: ComponentFixture<ConsultantTopNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultantTopNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultantTopNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
