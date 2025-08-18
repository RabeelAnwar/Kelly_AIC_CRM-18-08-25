import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ConsultantReferenceModal } from '../../../models/consultant/consultant-reference-model';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { Location } from '@angular/common';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-consultant-reference',
  templateUrl: './consultant-reference.component.html',
  styleUrl: './consultant-reference.component.css'
})
export class ConsultantReferenceComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,

    private ckConfig: CkeditorConfigService
  ) { }

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  consultantData: ConsultantModel = new ConsultantModel();
  referenceInput: ConsultantReferenceModal = new ConsultantReferenceModal();
  referenceList: ConsultantReferenceModal[] = [];

  ngOnInit(): void {

    const state = this.location.getState() as { consultant?: ConsultantModel };
    if (state.consultant) {
      this.consultantData = state.consultant;
      this.referenceInput.consultantId = state.consultant.id;
    }

  }

  saveConsultantReference() {

    //     if (!this.referenceInput.pipelineTypes || this.referenceInput.pipelineTypes.trim() === '') {
    //   this.toastr.warning('Type is required');
    //   return;
    // }

    this.apiService.saveData('Client/ReferenceAddUpdate', this.referenceInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Reference saved successfully');
          this.router.navigate(['/AllClientPipeline']);

        } else {
          this.toastr.error(res.message || 'Failed to save reference');
        }
      },
      error: () => this.toastr.error('Error saving reference')
    });

  }


  onCancel(): void {
    this.router.navigate(['/consultant-dashboard', this.consultantData.id]);

  }


  clientsList = [
    { name: 'Client 1', status: 'Active' },
    { name: 'Client 2', status: 'Inactive' },

  ]

}