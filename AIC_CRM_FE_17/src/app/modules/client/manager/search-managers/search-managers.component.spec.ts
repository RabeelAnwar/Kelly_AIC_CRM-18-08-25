import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchManagersComponent } from './search-managers.component';

describe('SearchManagersComponent', () => {
  let component: SearchManagersComponent;
  let fixture: ComponentFixture<SearchManagersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchManagersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchManagersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
