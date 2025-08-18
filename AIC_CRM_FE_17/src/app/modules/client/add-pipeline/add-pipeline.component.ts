import { Options } from '@angular-slider/ngx-slider';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../../services/api.service';
import { ClientPipelineModel } from '../../../models/client/client-pipeline-model';
import { ClientModel } from '../../../models/client/client-model';
import { DropdownItem } from '../../../models/common/common';
import { Location } from '@angular/common';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrl: './add-pipeline.component.css'
})
export class AddPipelineComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location

  ) { }

  pipelineInput: ClientPipelineModel = new ClientPipelineModel();
  pipelineList: ClientPipelineModel[] = [];

  clientData: ClientModel = new ClientModel();
  managers: DropdownItem[] = [];


  ngOnInit(): void {

    const state = this.location.getState() as { client?: ClientModel };
    if (state.client) {
      this.clientData = state.client;
    }

    this.pipelineInput.clientId = this.clientData?.id || 0;
    this.getManagers();
  }

  pipelineTypes = [
    { name: 'Email' },
    { name: 'Phone' },
    { name: 'SMS' },
    { name: 'Push Notification' }
  ];

  value: number = 0;
  options: Options = {
    ceil: 100,
    step: 5,
    showTicks: true,
    showTicksValues: true,
    animate: false,
    showSelectionBar: true
  };

  getManagers() {
    this.apiService.getData('Client/ClientManagersListGet').subscribe({
      next: (response) => {
        const dropdownItems: DropdownItem[] = [];

        response?.data.forEach((m: any) => {
          dropdownItems.push({
            id: m.id,
            name: m.firstName + ' ' + m.lastName,
          });
        });

        this.managers = dropdownItems;
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading managers list');
      }
    });
  }

  goToDashboard(): void {
    this.router.navigate(['/client-dashboard', this.clientData?.id]);
  }

  savePipeline(): void {
    if (!this.pipelineInput.pipelineTypes || this.pipelineInput.pipelineTypes.trim() === '') {
      this.toastr.warning('Type is required');
      return;
    }

    this.apiService.saveData('Client/ClientPipelineAddUpdate', this.pipelineInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Pipeline saved successfully');
          this.router.navigate(['/AllClientPipeline']);

        } else {
          this.toastr.error(res.message || 'Failed to save pipeline');
        }
      },
      error: () => this.toastr.error('Error saving pipeline')
    });
  }

}