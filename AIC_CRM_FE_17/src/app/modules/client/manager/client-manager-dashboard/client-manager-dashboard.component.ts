import { Component, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';
import { ApiService } from '../../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadModel } from '../../../../models/lead/lead-model';
import { ClientModel } from '../../../../models/client/client-model';
import { DocumentTypeModel } from '../../../../models/common/common';
import { DocumentUploadModel } from '../../../../models/admin/document-upload-model';
import { environment } from '../../../../../environment/environmemt';
import { FileDownload } from '../../../../services/file.download';
import { CallRecordModel } from '../../../../models/admin/call-record-model';
import { SelectItem } from 'primeng/api';
import { Location } from '@angular/common';
import { CkeditorConfigService } from '../../../../services/CkeditorConfigService';

@Component({
  selector: 'app-client-manager-dashboard',
  templateUrl: './client-manager-dashboard.component.html',
  styleUrl: './client-manager-dashboard.component.css',
})
export class ClientManagerDashboardComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private fileDownload: FileDownload,
    private location: Location,

    private ckConfig: CkeditorConfigService,
    private cdRef: ChangeDetectorRef // 🔄 Inject ChangeDetectorRef
  ) {}

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  callLogsInput: CallRecordModel = new CallRecordModel();
  callLogsList: CallRecordModel[] = [];
  searchCallLogs: string[] = [];
  searchLeads: string[] = [];
  callTypeList: SelectItem[] = [];
  fromClientDashboard: boolean = false;

  documentUploadInput: DocumentUploadModel = new DocumentUploadModel();
  documentUploadList: DocumentUploadModel[] = [];

  documentTypes: DocumentTypeModel[] = [];
  leadsList: LeadModel[] = [];
  clientManagersList: ClientManagerModel[] = [];
  fildteredManagersIds: number[] = [];

  managerId: number = 0;
  managerData: ClientManagerModel = new ClientManagerModel();
  clientData: ClientModel = new ClientModel();

  ngOnInit(): void {
    const state = this.location.getState() as { fromClientDashboard?: boolean };
    this.fromClientDashboard = state.fromClientDashboard || false;

    const stateM = this.location.getState() as {
      fildteredManagersIds?: number[];
    };

    if (
      stateM.fildteredManagersIds &&
      stateM.fildteredManagersIds?.length > 0
    ) {
      this.fildteredManagersIds = stateM.fildteredManagersIds;
    }

    // if (state.client) {
    //   this.clientInput = state.client;
    // }
    // This runs only once, but subscribes to route changes
    // this.route.params.subscribe((params) => {
    //   const id = +params['id']; // Convert id to number
    //   if (id > 0) {
    //     this.managerId = id;
    //     this.ClientManagerGet();
    //   }
    // });
    this.route.params.subscribe((params) => {
      const id = +params['id'];
      if (id > 0) {
        this.loadManagerData(id); // 👈 this calls both getManager and getCallLogs
      }
    });

    this.getDocumentTypes();
    this.getCallTypes();
    //this.getCallLogs();
  }

  ClientManagerGet() {
    this.apiService
      .getDataById('Client/ClientManagerGet', { id: this.managerId })
      .subscribe({
        next: (response) => {

          this.managerData = response?.data || new ClientManagerModel();

          const addressParts = [
            this.managerData.address1
              ? this.stripHtml(this.managerData.address1).trim()
              : '',
            this.managerData.city,
            this.managerData.state,
            this.managerData.country,
            this.managerData.zipCode,
          ];

          // Filter out null, undefined, or empty strings
          this.managerData.completeAddress = addressParts
            .filter((part) => part) // Removes falsy values: null, undefined, '', 0, false
            .join(', ');
            this.cdRef.detectChanges();

          this.ClientManagerGetByClientId();
          this.getLeads();
          this.getSingleClient();
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading clients list');
        },
      });
  }

  stripHtml(htmlString: any) {
    const temp = document.createElement('div');
    temp.innerHTML = htmlString;
    return temp.textContent || temp.innerText || '';
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
      error: () => this.toastr.error('Error fetching document types'),
    });
  }

  getLeads() {
    this.apiService.getData('Lead/LeadsListGet').subscribe({
      next: (response) => {
        this.leadsList = response?.data || [];
        this.leadsList = this.leadsList.filter(
          (x) => x.clientId === this.managerData.clientId
        );
        this.searchLeads = Object.keys(this.leadsList[0]);
      },
      error: (err) => {
        console.error('Error loading leads list', err);
      },
    });
  }

  getSingleClient() {
    this.apiService
      .getDataById('Client/SingleClientGet', { id: this.managerData.clientId })
      .subscribe({
        next: (response) => {
          this.clientData = response?.data || [];
          this.getUploadedFiles();
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading clients list');
        },
      });
  }

  ClientManagerGetByClientId() {
    this.apiService
      .getDataById('Client/ClientManagerGetByClientId', {
        id: this.managerData.clientId,
      })
      .subscribe({
        next: (response) => {
          this.clientManagersList = response?.data || [];
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading clients list');
        },
      });
  }

  goToClientDashboard(): void {
    this.router.navigate(['/client-dashboard', this.managerData.clientId]);
  }

  editManager(manager: ClientManagerModel) {
    this.router.navigate(['/AddManager'], {
      state: { manager, fromClientDashboard: this.fromClientDashboard },
    });
  }

  editLead(lead: LeadModel) {
    this.router.navigate(['/add-leads'], {
      state: { lead },
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
        },
      });
    }
  }

  UploadDocument(): void {
    this.documentUploadInput.clientId = this.managerData.clientId; // Set clientId from route parameter
    this.documentUploadInput.clientManagerId = this.managerId; // Set clientId from route parameter
    this.documentUploadInput.source = 'manager'; // Set source to 'client'

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
      if (
        model.hasOwnProperty(key) &&
        model[key] !== undefined &&
        model[key] !== null
      ) {
        if (key === 'documentFile' && model[key] instanceof File) {
          formData.append('DocumentFile', model[key]); // backend expects ResumeFile
        } else {
          formData.append(key, model[key]);
        }
      }
    }

    this.apiService
      .saveFormData('Admin/DocumentAddUpdate', formData)
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.toastr.success('Document uploaded successfully');
            this.clearDocument(); // Optional: reset form after success
            this.getUploadedFiles();
          } else {
            this.toastr.error(res.message || 'Failed to upload document');
          }
        },
        error: () => this.toastr.error('Error uploading document'),
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
      clientId: this.clientData.id, // replace with actual clientId
      managerId: this.managerId, // optional or default
      consultantId: 0, // optional or default
      requisitionId: 0, // optional or default
      source: 'manager',
    };

    this.apiService.getDataById('Admin/DocumentsListGet', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.documentUploadList = res.data;

          this.documentUploadList.forEach((doc) => {
            if (doc.documentFileName && doc.documentFileName.length > 0) {
              doc.documentFileName = `${environment.basePath}/${doc.documentFileName}`;
            }
          });
        }
      },
      error: (err) => {
        console.error('Error fetching documents:', err);
      },
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
        },
      });
    }
  }

  types = [
    { name: 'Email' },
    { name: 'Call' },
    { name: 'Text' },
    { name: 'Sales Meeting' },
  ];

  getCallTypes(): void {
    this.apiService.getData('Admin/CallTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.callTypeList = res.data;
          this.callLogsInput.date = new Date();
          // Default Type is Call
          this.callLogsInput.typeId = 2;

          console.log(res.data, 'call');
        } else {
          this.toastr.error('Failed to load call types');
        }
      },
      error: () => this.toastr.error('Error fetching call types'),
    });
  }

  /////////////////////////////   Call Record Section   /////////////////////////////

  saveCallRecord(): void {
    if (!this.callLogsInput.record) {
      this.toastr.warning('Call record is required');
      return;
    }

    if (!this.callLogsInput.typeId) {
      this.toastr.warning('Call type is required');
      return;
    }

    this.callLogsInput.managerId = this.managerId; // Set consultantId from route parameter

    this.apiService
      .saveData('Admin/AddOrUpdateCallRecord', this.callLogsInput)
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.getCallLogs();
            this.toastr.success('Saved successfully');
            this.clearCallRecord();
          } else {
            this.toastr.error(res.message || 'Failed to save');
          }
        },
        error: () => this.toastr.error('Error saving client'),
      });
  }

  @ViewChild('editor') editor: any;

  clearCallRecord(): void {
    this.callLogsInput.record = undefined;
    //this.editor?.quill?.setText('');
    this.callLogsInput = new CallRecordModel();
    this.callLogsInput.date = new Date();
    // Default Type is Call
    this.callLogsInput.typeId = 2;
    // Clear CKEditor content
    if (this.editor?.editorInstance) {
      this.editor.editorInstance.setData(''); // ✅ This clears CKEditor content
    }
  }

  editCallRecord(callRecord: CallRecordModel): void {
    this.callLogsInput = {
      ...callRecord,
      date: new Date(callRecord.date),
      reminderDate: callRecord.reminderDate
        ? new Date(callRecord.reminderDate)
        : undefined,
    };
  }

  deleteCallRecord(id: number): void {
    if (confirm('Are you sure you want to delete?')) {
      this.apiService.deleteData('Admin/DeleteCallRecord', { id }).subscribe({
        next: (response) => {
          this.toastr.success('Deleted successfully');
          this.getCallLogs();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting call record');
        },
      });
    }
  }

  getCallLogs(): void {
    const params = {
      leadsId: 0, // replace with actual clientId
      managerId: this.managerId, // optional or default
      consultantId: 0, // optional or default
    };

    this.apiService.getDataById('Admin/GetCallRecords', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.callLogsList = res.data;
          this.callLogsList = this.callLogsList.sort(
            (a: CallRecordModel, b: CallRecordModel) => {
              return new Date(b.date).getTime() - new Date(a.date).getTime();
            }
          );
          this.searchCallLogs = Object.keys(this.callLogsList[0]);
          console.log('searchCallLogs', this.searchCallLogs);
        } else {
          this.toastr.error('Failed to load call logs');
        }
      },
      error: () => this.toastr.error('Error fetching call logs'),
    });
  }

  clientsList: any[] = [];

  loadManagerData(id: number): void {
    this.managerId = id;
    this.ClientManagerGet();
    this.getCallLogs(); // ✅ Load call logs for the selected manager
  }
}
