import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftConsultantSearchNavComponent } from './left-consultant-search-nav.component';

describe('LeftConsultantSearchNavComponent', () => {
  let component: LeftConsultantSearchNavComponent;
  let fixture: ComponentFixture<LeftConsultantSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftConsultantSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftConsultantSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
