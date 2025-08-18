import { Component } from '@angular/core';
import { StateModel } from '../../../models/common/common';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-state',
  templateUrl: './state.component.html',
  styleUrl: './state.component.css'
})
export class StateComponent {


  stateList: StateModel[] = [];
  stateModel: StateModel = new StateModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getStates();
    this.stateModel.id = 0;
  }

  getStates(): void {
    this.apiService.getData('Admin/StateGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.stateList = res.data;
        } else {
          this.toastr.error('Failed to load states');
        }
      },
      error: () => this.toastr.error('Error fetching states')
    });
  }

  saveState(): void {
    const params = {
      Id: this.stateModel.id.toString(),
      name: this.stateModel.name
    };

    this.apiService.save('Admin/StateAddUpdate', params, {}).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('State saved successfully');
          this.getStates();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving state')
    });
  }

  deleteState(id: number): void {
    const params = { Id: id.toString() };

    this.apiService.deleteData('Admin/StateDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('State deleted successfully');
          this.getStates();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting state')
    });
  }

  editState(state: StateModel): void {
    this.stateModel = { ...state };
  }

  clearInput() {
    this.stateModel = new StateModel();
    this.stateModel.id = 0;
  }

}



