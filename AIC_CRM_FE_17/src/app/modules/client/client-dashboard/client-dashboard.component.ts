import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientModel } from '../../../models/client/client-model';
import { LeadModel } from '../../../models/lead/lead-model';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { DocumentTypeModel } from '../../../models/common/common';
import { DocumentUploadModel } from '../../../models/admin/document-upload-model';
import { FileDownload } from '../../../services/file.download';
import { environment } from '../../../../environment/environmemt';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-client-dashboard',
  templateUrl: './client-dashboard.component.html',
  styleUrl: './client-dashboard.component.css'
})
export class ClientDashboardComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private fileDownload: FileDownload,
    private authService: AuthService


  ) { }

  documentTypes: DocumentTypeModel[] = [];

  leadsList: LeadModel[] = [];
  requisitionsList: RequisitionModel[] = [];
  searchFieldsReq: string[] = [];
  clientId: number = 0;

  documentUploadInput: DocumentUploadModel = new DocumentUploadModel();
  documentUploadList: DocumentUploadModel[] = [];

  clientData: ClientModel = new ClientModel();
  clientManagersList: ClientManagerModel[] = [];

  ngOnInit(): void {
    // This runs only once, but subscribes to route changes
    this.route.params.subscribe(params => {
      const id = +params['id']; // Convert id to number

      if (id > 0) {
        this.clientId = id;
        this.getSingleClient();
        this.ClientManagerGetByClientId();
      }

    });

    this.getRequisitions();
    this.getDocumentTypes();
    this.getUploadedFiles();
  }


  getSingleClient() {
    this.apiService.getDataById('Client/SingleClientGet', { id: this.clientId }).subscribe({
      next: (response) => {
        this.clientData = response?.data || [];


        const addressParts = [
          this.clientData.address1 ? this.stripHtml(this.clientData.address1).trim() : '',
          this.clientData.city,
          this.clientData.state,
          this.clientData.country,
          this.clientData.zipCode
        ];

        // Filter out null, undefined, or empty strings
        this.clientData.completeAddress = addressParts
          .filter(part => part) // Removes falsy values: null, undefined, '', 0, false
          .join(', ');



      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  stripHtml(htmlString: any) {
    const temp = document.createElement('div');
    temp.innerHTML = htmlString;
    return temp.textContent || temp.innerText || '';
  }

  ClientManagerGetByClientId() {
    this.apiService.getDataById('Client/ClientManagerGetByClientId', { id: this.clientId }).subscribe({
      next: (response) => {
        this.clientManagersList = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  editClient(client: ClientModel) {
    this.router.navigate(['/add-client'], {
      state: { client }
    });
  }

  onRegisteredChange(event: any, client: ClientModel) {
    client.registered = event.target.checked;

    this.apiService.saveData('Client/ClientAddUpdate', client).subscribe({
      next: (response) => {
        // this.clientService.setClient(client);

        this.toastr.success('updated successfully');
      },
      error: (err) => {
        this.toastr.error(err || 'Failed to save update');
      }
    });
  }


  getLeads() {
    this.apiService.getData('Lead/LeadsListGet').subscribe({
      next: (response) => {
        this.leadsList = response?.data || [];
        this.leadsList = this.leadsList.filter(x => x.clientId === this.clientData.id);
      },
      error: (err) => {
        console.error('Error loading leads list', err);
      }
    });
  }


  getRequisitions() {
    this.apiService.getDataById('Client/ClientRequisitionsListGet', { clientId: this.clientId }).subscribe({
      next: (response) => {
        this.requisitionsList = response?.data || [];

        this.searchFieldsReq = Object.keys(this.requisitionsList[0]);
        console.log(this.requisitionsList);
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisition list');
      }
    });
  }


  editLead(lead: LeadModel) {
    this.router.navigate(['/add-leads'], {
      state: { lead }
    });
  }

  deleteLead(id: number) {
    if (confirm('Are you sure you want to delete this lead?')) {
      this.apiService.deleteData('Lead/LeadDelete', { id }).subscribe({
        next: (response) => {
          console.log('Lead deleted successfully');
          this.getLeads();
        },
        error: (err) => {
          console.error('Error deleting lead', err);
        }
      });
    }
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


  UploadDocument(): void {
    this.documentUploadInput.clientId = this.clientId; // Set clientId from route parameter
    this.documentUploadInput.source = 'client'; // Set source to 'client'

    // Validation checks
    if (!this.documentUploadInput) {
      this.toastr.warning('Document Type is required');
      return;
    }

    if (!this.documentUploadInput) {
      this.toastr.warning('Document file is required');
      return;
    }

    const formData = new FormData();
    const model = this.documentUploadInput as any; // Type assertion to bypass TS7053

    for (const key in model) {
      if (model.hasOwnProperty(key) && model[key] !== undefined && model[key] !== null) {
        if (key === 'documentFile' && model[key] instanceof File) {
          formData.append('DocumentFile', model[key]); // backend expects ResumeFile
        } else {
          formData.append(key, model[key]);
        }
      }
    }

    this.apiService.saveFormData('Admin/DocumentAddUpdate', formData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Document uploaded successfully');
          this.clearDocument(); // Optional: reset form after success
          this.getUploadedFiles();
        } else {
          this.toastr.error(res.message || 'Failed to upload document');
        }
      },
      error: () => this.toastr.error('Error uploading document')
    });
  }


  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.documentUploadInput.documentFile = target.files[0];
    }
  }


  clearDocument(): void {
    this.documentUploadInput = new DocumentUploadModel();
  }


  downloadFile(fileUrl?: string) {

    if (fileUrl && fileUrl.length > 0) {
      this.fileDownload.downloadFileByUrl(fileUrl);
    }
  }

  getUploadedFiles(): void {

    const params = {
      clientId: this.clientId, // replace with actual clientId
      managerId: 0,  // optional or default
      consultantId: 0, // optional or default
      requisitionId: 0, // optional or default
      source: 'client'
    };

    this.apiService.getDataById('Admin/DocumentsListGet', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.documentUploadList = res.data;

          this.documentUploadList.forEach(doc => {
            if (doc.documentFileName && doc.documentFileName.length > 0) {
              doc.documentFileName = `${environment.basePath}/${doc.documentFileName}`;
            }
          });
        }
      },
      error: err => {
        console.error('Error fetching documents:', err);
      }
    });
  }

  deleteDocument(id: number) {
    if (confirm('Are you sure you want to delete?')) {
      this.apiService.deleteData('Admin/DocumentDelete', { id }).subscribe({
        next: (response) => {
          this.toastr.success('Deleted successfully');
          this.getUploadedFiles();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting client');
        }
      });
    }
  }

  clientsList: any[] = [];



  requisitionDashboard(requisition: RequisitionModel) {
    this.router.navigate(['/RequisitionDashboard', requisition.id], {
      state: { requisition }
    });
  }
  goToManagerDashboard(requisition: RequisitionModel) {
    this.router.navigate(['/ManagerDashboard', requisition?.managerId]);
  }


  addRequisition(data: RequisitionModel) {
    const requisition = new RequisitionModel();
    ;
    requisition.clientId = data?.clientId;
    requisition.salesRepId = this.authService.getUserId();
    requisition.internalReqCoordinatorId = this.authService.getUserId();
    this.router.navigate(['/ClientITRequisition'], {
      state: { requisition: requisition }
    });
  }

  editRequisition(requisition: RequisitionModel) {
    this.router.navigate(['/ClientITRequisition'], {
      state: { requisition }
    });
  }


  deleteRequisition(id: number) {
    if (confirm('Are you sure you want to delete this requisition?')) {
      this.apiService.deleteData('Client/ClientRequisitionDelete', { id }).subscribe({
        next: () => {
          this.toastr.success('Requisition deleted successfully');
          this.getRequisitions();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting requisition');
        }
      });
    }
  }

  onStatusChange(event: any, rowData: RequisitionModel) {
    rowData.status = event.target.checked;

    this.apiService.saveData('Client/ClientRequisitionStatusUpdate', rowData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('saved successfully');
        } else {
          this.toastr.error(res.message || 'Failed to save');
        }
      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error saving');
      }
    });
  }

}