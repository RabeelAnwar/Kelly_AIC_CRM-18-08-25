import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DocumentTypeModel, DropdownItem } from '../../../models/common/common';
import { ItemsList } from '@ng-select/ng-select/lib/items-list';

@Component({
  selector: 'app-document-type',
  templateUrl: './document-type.component.html',
  styleUrl: './document-type.component.css'
})
export class DocumentTypeComponent {

  documentTypes: DocumentTypeModel[] = [];

  userTypes: DropdownItem[] = [
    { id: 1, name: "Consultant" },
    { id: 2, name: "Manager" },
    { id: 3, name: "Client" },

  ];

  documentModel: DocumentTypeModel = new DocumentTypeModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getDocumentTypes();
  }

  userTypeGet(doc: any): string {
    const user = this.userTypes.find(u => u.id === doc.userTypeId);
    return user ? user.name : 'Unknown';
  }

  getDocumentTypes(): void {
    this.apiService.getData('Admin/DocumentTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.documentTypes = res.data;
        } else {
          this.toastr.error('Failed to load document types');
        }
      },
      error: () => this.toastr.error('Error fetching document types')
    });
  }

  saveDocumentType(): void {

    if (!this.documentModel.userTypeId || !this.documentModel.documentTypeName || this.documentModel.documentTypeName.trim() === '') {
      this.toastr.warning('All fields are required');
      return;
    }
    this.apiService.saveData('Admin/DocumentTypeAddUpdate', this.documentModel).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Document type saved successfully');
          this.getDocumentTypes();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving document type')
    });
  }

  deleteDocumentType(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this document type?');
    if (!confirmDelete) {
      return;
    }

    this.apiService.deleteData('Admin/DocumentTypeDelete', { id: id }).subscribe({

      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Document type deleted successfully');
          this.getDocumentTypes();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting document type')
    });
  }

  editDocumentType(doc: DocumentTypeModel): void {
     
    this.documentModel = { ...doc };
  }

  clearInput(): void {
    this.documentModel = new DocumentTypeModel();
  }

}


