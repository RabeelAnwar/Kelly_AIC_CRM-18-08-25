import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientTopNavComponent } from './client-top-nav.component';

describe('ClientTopNavComponent', () => {
  let component: ClientTopNavComponent;
  let fixture: ComponentFixture<ClientTopNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientTopNavComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ClientTopNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
