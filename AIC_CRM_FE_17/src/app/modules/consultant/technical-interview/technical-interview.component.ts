import { Component } from '@angular/core';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ConsultantInterviewModal } from '../../../models/consultant/consultant-interview-model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-technical-interview',
  templateUrl: './technical-interview.component.html',
  styleUrl: './technical-interview.component.css'
})
export class TechnicalInterviewComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location

  ) { }

  consultantData: ConsultantModel = new ConsultantModel();

  interviewInput: ConsultantInterviewModal = new ConsultantInterviewModal();
  interviewList: ConsultantInterviewModal[] = [];


  ngOnInit(): void {

     
    const state = this.location.getState() as { consultant?: ConsultantModel };
    if (state.consultant) {
      this.consultantData = state.consultant;
      this.interviewInput.consultantId = state.consultant.id;
    }

  }

  saveConsultantInterview() {

    //     if (!this.interviewInput.pipelineTypes || this.interviewInput.pipelineTypes.trim() === '') {
    //   this.toastr.warning('Type is required');
    //   return;
    // }

    this.apiService.saveData('Client/InterviewAddUpdate', this.interviewInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Interview saved successfully');
          this.router.navigate(['/AllClientPipeline']);

        } else {
          this.toastr.error(res.message || 'Failed to save interview');
        }
      },
      error: () => this.toastr.error('Error saving interview')
    });

  }

  onCancel() {
    this.interviewInput = new ConsultantInterviewModal();
    this.interviewInput.consultantId = this.consultantData?.id || 0;
  }

  clientsList = [
    { name: 'Client 1', status: 'Active' },
    { name: 'Client 2', status: 'Inactive' },
  ]

}
