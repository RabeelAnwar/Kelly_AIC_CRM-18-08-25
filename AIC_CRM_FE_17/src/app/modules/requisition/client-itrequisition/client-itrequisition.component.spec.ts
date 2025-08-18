import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientITRequisitionComponent } from './client-itrequisition.component';

describe('ClientITRequisitionComponent', () => {
  let component: ClientITRequisitionComponent;
  let fixture: ComponentFixture<ClientITRequisitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientITRequisitionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ClientITRequisitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
