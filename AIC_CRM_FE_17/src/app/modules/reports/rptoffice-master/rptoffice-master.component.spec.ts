import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RPTOfficeMasterComponent } from './rptoffice-master.component';

describe('RPTOfficeMasterComponent', () => {
  let component: RPTOfficeMasterComponent;
  let fixture: ComponentFixture<RPTOfficeMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RPTOfficeMasterComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RPTOfficeMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
