import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RightManagerSearchNavComponent } from './right-manager-search-nav.component';

describe('RightManagerSearchNavComponent', () => {
  let component: RightManagerSearchNavComponent;
  let fixture: ComponentFixture<RightManagerSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RightManagerSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RightManagerSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
