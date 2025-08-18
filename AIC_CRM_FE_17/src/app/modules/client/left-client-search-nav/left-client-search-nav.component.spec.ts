import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftClientSearchNavComponent } from './left-client-search-nav.component';

describe('LeftClientSearchNavComponent', () => {
  let component: LeftClientSearchNavComponent;
  let fixture: ComponentFixture<LeftClientSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftClientSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftClientSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
