import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchConsultantComponent } from './search-consultant.component';

describe('SearchConsultantComponent', () => {
  let component: SearchConsultantComponent;
  let fixture: ComponentFixture<SearchConsultantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchConsultantComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchConsultantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
