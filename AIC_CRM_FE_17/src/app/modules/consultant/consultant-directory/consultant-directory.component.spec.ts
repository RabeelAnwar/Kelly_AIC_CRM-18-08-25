import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultantDirectoryComponent } from './consultant-directory.component';

describe('ConsultantDirectoryComponent', () => {
  let component: ConsultantDirectoryComponent;
  let fixture: ComponentFixture<ConsultantDirectoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultantDirectoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConsultantDirectoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
