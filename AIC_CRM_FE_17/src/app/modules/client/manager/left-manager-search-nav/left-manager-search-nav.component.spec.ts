import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftManagerSearchNavComponent } from './left-manager-search-nav.component';

describe('LeftManagerSearchNavComponent', () => {
  let component: LeftManagerSearchNavComponent;
  let fixture: ComponentFixture<LeftManagerSearchNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftManagerSearchNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftManagerSearchNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
