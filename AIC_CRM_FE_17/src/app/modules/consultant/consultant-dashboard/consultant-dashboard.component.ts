import { Component, ElementRef, ViewChild } from '@angular/core';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environment/environmemt';
import { FileDownload } from '../../../services/file.download';
import { DocumentTypeModel } from '../../../models/common/common';
import * as mammoth from 'mammoth';
import { HttpClient } from '@angular/common/http';
import { DocumentUploadModel } from '../../../models/admin/document-upload-model';
import { CallRecordModel } from '../../../models/admin/call-record-model';
import { Location } from '@angular/common';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-consultant-dashboard',
  templateUrl: './consultant-dashboard.component.html',
  styleUrl: './consultant-dashboard.component.css',
})
export class ConsultantDashboardComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private fileDownload: FileDownload,
    private http: HttpClient,
    private location: Location,

    private ckConfig: CkeditorConfigService
  ) {}

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  @ViewChild('fileInput') fileInput!: ElementRef;
  isLoading = false;

  resumeSearchHighLight?: string;
  fildteredConsultantsIds: number[] = [];

  documentTypes: DocumentTypeModel[] = [];

  consultantParamId: any;
  resumeHtml: string | null = null;
  consultantData: ConsultantModel = new ConsultantModel();

  documentUploadInput: DocumentUploadModel = new DocumentUploadModel();
  documentUploadList: DocumentUploadModel[] = [];

  callLogsInput: CallRecordModel = new CallRecordModel();
  callLogsList: CallRecordModel[] = [];
  searchCallLogs: any[] = [];

  ngOnInit(): void {
    // This runs only once, but subscribes to route changes
    this.route.params.subscribe((params) => {
      const id = +params['id']; // Convert id to number

      this.consultantParamId = id;

      const state = this.location.getState() as {
        resumeSearchHighLight?: string;
        fildteredConsultantsIds?: number[];
      };
      if (state.resumeSearchHighLight) {
        this.resumeSearchHighLight = state.resumeSearchHighLight;
      }

      if (
        state.fildteredConsultantsIds &&
        state.fildteredConsultantsIds?.length > 0
      ) {
        this.fildteredConsultantsIds = state.fildteredConsultantsIds;
      }

      this.SingleConsultantGet();
    });

    this.getDocumentTypes();
    this.getUploadedFiles();
    this.getCallLogs();

    this.callLogsInput.date = new Date();
    // Default Type is Call
    this.callLogsInput.typeId = 2;
  }

  onResumeSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.consultantData.resumeFile = input.files[0];
      this.saveConsultant();
    }
  }

  requisitionDashboard(id: number) {
    this.router.navigate(['/RequisitionDashboard', id]);
  }

  saveConsultant(): void {
    if (
      !this.consultantData.firstName ||
      this.consultantData.firstName.trim() === ''
    ) {
      this.toastr.warning('First Name is required');
      return;
    }

    if (
      !this.consultantData.lastName ||
      this.consultantData.lastName.trim() === ''
    ) {
      this.toastr.warning('Last Name is required');
      return;
    }
    this.isLoading = true;
    const formData = new FormData();
    const model = this.consultantData as any;

    for (const key in model) {
      if (
        model.hasOwnProperty(key) &&
        model[key] !== undefined &&
        model[key] !== null
      ) {
        if (key === 'resumeFile' && model[key] instanceof File) {
          formData.append('ResumeFile', model[key]);
        } else {
          formData.append(key, model[key]);
        }
      }
    }
    this.apiService
      .saveFormData('Consultant/ConsultantAddUpdate', formData)
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.toastr.success('Consultant saved successfully');
            this.SingleConsultantGet();
          } else {
            this.toastr.error(res.message || 'Failed to save consultant');
          }
          this.isLoading = false;
        },
        error: () => {
          this.toastr.error('Error saving consultant');
          this.isLoading = false;
        },
      });
  }

  deleteConsultantFile() {
    // Check if resume is empty or undefined
    if (
      !this.consultantData.resume ||
      this.consultantData.resume.length === 0
    ) {
      return;
    }

    // Ask user for confirmation
    if (confirm('Are you sure you want to delete?')) {
      this.apiService
        .deleteData('Consultant/DeleteConsultantFileAsync', {
          id: this.consultantParamId,
        })
        .subscribe({
          next: () => {
            this.SingleConsultantGet(); // Refresh consultant data
            if (this.fileInput) {
              this.fileInput.nativeElement.value = '';
            }
            this.toastr.success('Deleted successfully');
          },
          error: (err) => {
            const errorMessage = err?.message || 'Error deleting consultant';
            this.toastr.error(errorMessage);
          },
        });
    }
  }

  SingleConsultantGet() {
    this.apiService
      .getDataById('Consultant/SingleConsultantGet', {
        id: this.consultantParamId,
      })
      .subscribe({
        next: (response) => {
          this.consultantData = response?.data || [];
          if (
            this.consultantData.resume &&
            this.consultantData.resume.length > 0
          ) {
            this.consultantData.resume =
              `${environment.basePath}/` + this.consultantData.resume;
            this.loadResumeWordFile(this.consultantData.resume);
          } else {
            this.resumeHtml = null; // Reset if no resume is available
          }

          const addressParts = [
            this.consultantData.address1
              ? this.stripHtml(this.consultantData.address1).trim()
              : '',
            this.consultantData.city,
            this.consultantData.state,
            this.consultantData.country,
            this.consultantData.zipCode,
          ];

          // Filter out null, undefined, or empty strings
          this.consultantData.completeAddress = addressParts
            .filter((part) => part) // Removes falsy values: null, undefined, '', 0, false
            .join(', ');
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading consultants list');
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

  loadResumeWordFile(url: string) {
    this.isLoading = true;
    this.apiService
      .getDataByName('Consultant/ExtractText', { url: url })
      .subscribe({
        next: (res: any) => {
          this.isLoading = false;
          if (res.succeeded) {
            this.resumeHtml = res.data;
            this.resumeSearchWordsHighLight();
          } else {
            this.toastr.error('Failed to load document types');
          }
        },
        error: () => {
          this.toastr.error('Error fetching document types');
          this.isLoading = false;
        },
      });

    // this.isLoading = true;
    // this.http.get(url, { responseType: 'arraybuffer' }).subscribe({
    //   next: (arrayBuffer) => {
    //     this.isLoading = false;
    //     mammoth.convertToHtml({ arrayBuffer }).then((result) => {
    //       this.resumeHtml = result.value;
    //       this.resumeSearchWordsHighLight();
    //     }).catch((err) => {
    //       console.error('Error rendering resume:', err);
    //     });
    //   },
    //   error: (err) => {
    //     console.error('Error fetching resume:', err);
    //     this.isLoading = false;

    //   }
    // });
  }

  resumeSearchWordsHighLight() {
    const searchPhrase = this.resumeSearchHighLight?.toString().trim();
    if (!searchPhrase || !this.resumeHtml) return;

    const searchWords = searchPhrase.toLowerCase().split(/\s+/).filter(Boolean);

    // Escape regex characters and create a regex pattern to match all words
    const escapedWords = searchWords.map((word) =>
      word.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
    );

    // \b ensures word boundaries; 'gi' for global + case-insensitive
    const regex = new RegExp(`\\b(${escapedWords.join('|')})\\b`, 'gi');

    // Replace matches with <mark> tags
    this.resumeHtml = this.resumeHtml.replace(regex, '<mark>$1</mark>');
  }

  UploadDocument(): void {
    this.documentUploadInput.consultantId = this.consultantParamId; // Set clientId from route parameter
    this.documentUploadInput.source = 'consultant'; // Set source to 'client'

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
      clientId: 0, // replace with actual clientId
      managerId: 0, // optional or default
      consultantId: this.consultantParamId, // optional or default
      requisitionId: 0, // optional or default
      source: 'consultant',
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

  /////////////////////////////   Call Record Section   /////////////////////////////

  saveCallRecord(): void {
    if (!this.callLogsInput.record) {
      this.toastr.warning('Call record is required');
      return;
    }

    this.callLogsInput.consultantId = this.consultantParamId; // Set consultantId from route parameter

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
    this.editor?.quill?.setText('');
    this.callLogsInput = new CallRecordModel();

    this.callLogsInput.date = new Date();
    // Default Type is Call
    this.callLogsInput.typeId = 2;
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
      managerId: 0, // optional or default
      consultantId: this.consultantParamId, // optional or default
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
        } else {
          this.toastr.error('Failed to load call logs');
        }
      },
      error: () => this.toastr.error('Error fetching call logs'),
    });
  }
}
