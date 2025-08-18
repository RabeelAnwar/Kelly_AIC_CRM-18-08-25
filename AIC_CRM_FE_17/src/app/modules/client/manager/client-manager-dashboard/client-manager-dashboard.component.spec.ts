import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientManagerDashboardComponent } from './client-manager-dashboard.component';

describe('ClientManagerDashboardComponent', () => {
  let component: ClientManagerDashboardComponent;
  let fixture: ComponentFixture<ClientManagerDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientManagerDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ClientManagerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
