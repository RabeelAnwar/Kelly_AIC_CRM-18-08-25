import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllClientPipelineComponent } from './all-client-pipeline.component';

describe('AllClientPipelineComponent', () => {
  let component: AllClientPipelineComponent;
  let fixture: ComponentFixture<AllClientPipelineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllClientPipelineComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AllClientPipelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
