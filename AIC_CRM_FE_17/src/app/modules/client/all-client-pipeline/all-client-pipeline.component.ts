import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ClientPipelineModel } from '../../../models/client/client-pipeline-model';
import { ApiService } from '../../../services/api.service';
import { CustomToastService } from '../../../services/custom-toast.service';

@Component({
  selector: 'app-all-client-pipeline',
  templateUrl: './all-client-pipeline.component.html',
  styleUrl: './all-client-pipeline.component.css'
})
export class AllClientPipelineComponent {

  pipelineList: ClientPipelineModel[] = [];
  searchFields: string[] = [];

  constructor(private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,

  ) { }

  ngOnInit(): void {
    this.getPipeline();
  }

  getPipeline() {
    this.apiService.getData('Client/ClientPipelinesListGet').subscribe({
      next: (response) => {
        this.pipelineList = response?.data || [];
        console.log(this.pipelineList);

        this.searchFields = Object.keys(this.pipelineList);

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading pipelines list');
      }
    });
  }



  deletePipeline(id: number) {
    if (confirm('Are you sure you want to delete this pipeline?')) {
      this.apiService.deleteData('Client/ClientPipelineDelete', { id }).subscribe({
        next: (response) => {
          this.toastr.success('Pipeline deleted successfully');
          this.getPipeline();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting pipeline');
        }
      });
    }
  }
}
