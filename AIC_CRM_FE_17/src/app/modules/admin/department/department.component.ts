import { Component } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { DepartmentModel } from '../../../models/common/common';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrl: './department.component.css'
})
export class DepartmentComponent {


  departmentList: SelectItem[] = [];
  departmentModel: DepartmentModel = new DepartmentModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    this.getDepartments();
  }

  getDepartments(): void {
    this.apiService.getData('Admin/DepartmentGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.departmentList = res.data;
        } else {
          this.toastr.error('Failed to load departments');
        }
      },
      error: () => this.toastr.error('Error fetching departments')
    });
  }

  saveDepartment(): void {

    if (!this.departmentModel.name || this.departmentModel.name.trim() === '') {
      this.toastr.warning('Department Name is required');
      return;
    }

    this.apiService.saveData('Admin/DepartmentAddUpdate', this.departmentModel).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('department saved successfully');
          this.getDepartments();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving department')
    });
  }

  deleteDepartment(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this department?');
    if (!confirmDelete) {
      return;
    }

    const params = { Id: id.toString() };

    this.apiService.deleteData('Admin/DepartmentDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('department deleted successfully');
          this.getDepartments();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting department')
    });

  }


  editDepartment(department: DepartmentModel): void {
    this.departmentModel = { ...department };
  }

  clearInput() {
    this.departmentModel = new DepartmentModel();
  }


}
