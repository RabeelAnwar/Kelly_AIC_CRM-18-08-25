import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPipelineComponent } from './add-pipeline.component';

describe('AddPipelineComponent', () => {
  let component: AddPipelineComponent;
  let fixture: ComponentFixture<AddPipelineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPipelineComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddPipelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
