import { Component } from '@angular/core';
import { ContactTypeModel } from '../../../models/common/common';
import { SelectItem } from 'primeng/api';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contact-type',
  templateUrl: './contact-type.component.html',
  styleUrl: './contact-type.component.css'
})
export class ContactTypeComponent {

  
  contactTypeList: SelectItem[] = [];
  contactTypeModel: ContactTypeModel = new ContactTypeModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getContactTypes();
  }

  getContactTypes(): void {
    this.apiService.getData('Admin/ContactTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.contactTypeList = res.data;
        } else {
          this.toastr.error('Failed to load contact types');
        }
      },
      error: () => this.toastr.error('Error fetching contact types')
    });
  }

  saveContactType(): void {

    if (!this.contactTypeModel.name || this.contactTypeModel.name.trim() === '') {
      this.toastr.warning('Contact Type Name is required');
      return;
    }

    this.apiService.saveData('Admin/ContactTypeAddUpdate', this.contactTypeModel).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Contact type saved successfully');
          this.getContactTypes();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving contact type')
    });
  }

  deleteContactType(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this contact type?');
    if (!confirmDelete) {
      return;
    }
    
    const params = { Id: id.toString() };

    this.apiService.deleteData('Admin/ContactTypeDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Contact type deleted successfully');
          this.getContactTypes();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting contact type')
    });
  }

  editContactType(contactType: ContactTypeModel): void {
    this.contactTypeModel = { ...contactType };
  }

  clearInput(): void {
    this.contactTypeModel = new ContactTypeModel();
  }

}
