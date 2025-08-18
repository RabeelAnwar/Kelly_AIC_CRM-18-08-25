import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { FileDownload } from '../../../services/file.download';
import { ClientModel } from '../../../models/client/client-model';
import { Location } from '@angular/common';
import { environment } from '../../../../environment/environmemt';
import { DocumentUploadModel } from '../../../models/admin/document-upload-model';
import { DocumentTypeModel } from '../../../models/common/common';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { LeadModel } from '../../../models/lead/lead-model';
import { ClientManagerModel } from '../../../models/client/client-manager-model';

@Component({
  selector: 'app-requisition-dashboard',
  templateUrl: './requisition-dashboard.component.html',
  styleUrl: './requisition-dashboard.component.css'
})
export class RequisitionDashboardComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private fileDownload: FileDownload,
  ) { }

  documentUploadList: DocumentUploadModel[] = [];
  documentUploadInput: DocumentUploadModel = new DocumentUploadModel();
  documentTypes: DocumentTypeModel[] = [];
  interviewProcessConsultantsList: any[] = [];

  searchFields: string[] = []

  clientData: ClientModel = new ClientModel();
  requisitionData: RequisitionModel = new RequisitionModel();
  requisitionList: RequisitionModel[] = [];
  requisitionId: number = 0;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id']; // Convert id to number

      if (id > 0) {
        this.requisitionId = id;

        // Load from route state if available
        // const state = this.location.getState() as { requisition?: RequisitionModel };
        // if (state.requisition && state.requisition.id === id) {
        //   this.requisitionData = state.requisition;
        // } else if (state.requisition && state.requisition.clientId) {
        //   this.requisitionData.clientId = state.requisition.clientId;
        // }

        this.SingleRequisitionGet();
        this.getDocumentTypes();
        this.getUploadedFiles();
        this.getInterviewProcessConsultantsList();
      }
    });

  }

  getSingleClient() {
    this.apiService.getDataById('Client/SingleClientGet', { id: this.requisitionData.clientId }).subscribe({
      next: (response) => {
        this.clientData = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  getRequisitionGetByClientId() {
    this.apiService.getDataById('Client/ClientRequisitionGetByClientId', { id: this.requisitionData.clientId }).subscribe({
      next: (response) => {
        this.requisitionList = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisitions list');
      }
    });
  }


  SingleRequisitionGet() {
    this.apiService.getDataById('Client/ClientRequisitionGet', { id: this.requisitionId }).subscribe({
      next: (response) => {
        this.requisitionData = response?.data || [];
        console.log('Requisition Data:', response?.data);
        this.getSingleClient();
        this.getRequisitionGetByClientId();
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisition');
      }
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
          console.log('Lead deleted successfully');
          this.router.navigate(['/AllRequisition']);
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting requisition');
        }
      });
    }
  }

  clientDashboard() {
    this.router.navigate(['/client-dashboard', this.requisitionData.clientId]);
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
    this.documentUploadInput.requisitionId = this.requisitionId; // Set clientId from route parameter
    this.documentUploadInput.source = 'requisition'; // Set source to 'client'

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
      clientId: 0, // replace with actual clientId
      managerId: 0,  // optional or default
      consultantId: 0, // optional or default
      requisitionId: this.requisitionId, // optional or default
      source: 'requisition'
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


  getInterviewProcessConsultantsList(): void {
    this.apiService.getDataById('Consultant/InterviewProcessConsultantsList', { requisitionId: this.requisitionId }).subscribe({
      next: (response) => {
        if (response.succeeded) {

          this.interviewProcessConsultantsList = response.data;
          this.searchFields = Object.keys(this.interviewProcessConsultantsList[0]);

        } else {
          this.toastr.error('Failed to load candidates');
        }

      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error loading candidates');
      }
    });
  }


  interviewProcess(id: number, processId?: number) {
    localStorage.setItem('activityId', id.toString());
    this.router.navigate(['/ReqConsultantInterviewProcess'], {
      state: { requisition: this.requisitionData, processId: processId }
    });

  }

  consultantActivity() {

    localStorage.setItem('activityId', '');
    this.router.navigate(['/ConsultantActivity'], {
      state: { requisition: this.requisitionData }
    });
  }


  onStatusChange(event: any) {
    this.requisitionData.status = event.target.checked;

    this.apiService.saveData('Client/ClientRequisitionStatusUpdate', this.requisitionData).subscribe({
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


  consultantDashboard(id: number) {
    this.router.navigate(['/consultant-dashboard', id]);
  }

  hasSelectedStatus(statusList: string[]): boolean {
    return statusList.includes('Selected');
  }

  calculatePercentage(billRate: number, payRate: number): string {
    if (isNaN(billRate) || isNaN(payRate) || payRate === null || billRate === null || payRate === 0) {
      return '0';
    }
    const percentage = ((billRate - payRate) / payRate) * 100;
    return percentage.toFixed(2); // you can adjust decimal places as per your need
  }


  showMore: { [key: string]: boolean } = {
    projectOverview: false,
    jobDescription: false,
    responsibilities: false,
    qualifications: false
  };

  // Toggle function for each section
  toggleShowMore(field: string) {
    this.showMore[field] = !this.showMore[field];
  }

  deleteConsultantActivity(id: number): void {
    if (confirm('Are you sure you want to delete this activity?')) {
      this.apiService.deleteData('Consultant/ConsultantActivityDelete', { id: id }).subscribe({
        next: (res) => {
          this.toastr.success('Deleted successfully');
          this.getInterviewProcessConsultantsList();
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error deleting activity');
        }
      });
    }
  }


  addManager() {
    const manager = new ClientManagerModel();
    manager.clientId = this.clientData.id;
    manager.address1 = this.clientData?.address1;
    manager.address2 = this.clientData?.address2;

    manager.country = this.clientData?.country;
    manager.state = this.clientData?.state;
    manager.city = this.clientData?.city;
    manager.zipCode = this.clientData?.zipCode;

    this.router.navigate(['/AddManager'], {
      state: { manager: manager }
    });
  }

  addLead() {
    const lead = new LeadModel();
    lead.clientId = this.clientData.id;
    this.router.navigate(['/add-leads'], {
      state: { lead: lead }
    });
  }

}