import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RPTIndividualComponent } from './rptindividual.component';

describe('RPTIndividualComponent', () => {
  let component: RPTIndividualComponent;
  let fixture: ComponentFixture<RPTIndividualComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RPTIndividualComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RPTIndividualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
