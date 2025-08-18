import { Component } from '@angular/core';

import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { CallTypeModel } from '../../../models/common/common';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-call-type',
  templateUrl: './call-type.component.html',
  styleUrl: './call-type.component.css'
})
export class CallTypeComponent {

  callTypeList: SelectItem[] = [];
  callTypeModel: CallTypeModel = new CallTypeModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getCallTypes();
  }

  getCallTypes(): void {
    this.apiService.getData('Admin/CallTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.callTypeList = res.data;
        } else {
          this.toastr.error('Failed to load call types');
        }
      },
      error: () => this.toastr.error('Error fetching call types')
    });
  }

  saveCallType(): void {

    if (!this.callTypeModel.name || this.callTypeModel.name.trim() === '') {
      this.toastr.warning('Call Type Name is required');
      return;
    }

    this.apiService.saveData('Admin/CallTypeAddUpdate', this.callTypeModel).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Call type saved successfully');
          this.getCallTypes();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving call type')
    });
  }

  deleteCallType(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this call type?');
    if (!confirmDelete) {
      return;
    }

    const params = { Id: id.toString() };

    this.apiService.deleteData('Admin/CallTypeDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Call type deleted successfully');
          this.getCallTypes();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting call type')
    });
  }

  editCallType(callType: CallTypeModel): void {
    this.callTypeModel = { ...callType };
  }

  clearInput(): void {
    this.callTypeModel = new CallTypeModel();
  }

}
