import { Component, ViewChild } from '@angular/core';
import { LeadListModel, LeadModel } from '../../../models/lead/lead-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientModel } from '../../../models/client/client-model';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { Location } from '@angular/common';
import { CallRecordModel } from '../../../models/admin/call-record-model';
import { SelectItem } from 'primeng/api';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-lead-dashboard',
  templateUrl: './lead-dashboard.component.html',
  styleUrl: './lead-dashboard.component.css'
})
export class LeadDashboardComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,

    private ckConfig: CkeditorConfigService
  ) { }

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  managerData: ClientManagerModel = new ClientManagerModel();
  clientData: ClientModel = new ClientModel();
  leadData: LeadListModel = new LeadListModel();
  leadId: number = 0;


  callLogsInput: CallRecordModel = new CallRecordModel();
  callLogsList: CallRecordModel[] = [];
  searchCallLogs: any[] = [];
  callTypeList: SelectItem[] = [];

  ngOnInit(): void {
    // This runs only once, but subscribes to route changes
    this.route.params.subscribe(params => {
      const id = +params['id']; // Convert id to number

      if (id > 0) {
        this.leadId = id;
      }

      const state = this.location.getState() as { lead?: LeadListModel };
      if (state.lead && state.lead.id === id) {

        if (state.lead.id > 0) {
          this.leadData = state.lead;
        }
        else {
          this.leadData.clientId = state.lead.clientId;
        }

        this.getSingleClient();
        this.getManagers();

      }

    });

    this.getCallTypes();
    this.getCallLogs();

  }

  getSingleClient() {
    this.apiService.getDataById('Client/SingleClientGet', { id: this.leadData.clientId }).subscribe({
      next: (response) => {
        this.clientData = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  getManagers() {
    this.apiService.getDataById('Client/ClientManagerGet', { id: this.leadData.managerId }).subscribe({
      next: (response) => {
        this.managerData = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading managers list');
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
          this.router.navigate(['/all-leads']);
        },
        error: (err) => {
          console.error('Error deleting lead', err);
        }
      });
    }
  }

  clientDashboard() {
    this.router.navigate(['/client-dashboard', this.clientData.id]);
  }

  getCallTypes(): void {
    this.apiService.getData('Admin/CallTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.callTypeList = res.data;
          this.callLogsInput.date = new Date();
          // Default Type is Call
          this.callLogsInput.typeId = 2
        } else {
          this.toastr.error('Failed to load call types');
        }
      },
      error: () => this.toastr.error('Error fetching call types')
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

    this.callLogsInput.leadId = this.leadId; // Set consultantId from route parameter

    this.apiService.saveData('Admin/AddOrUpdateCallRecord', this.callLogsInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.getCallLogs();
          this.toastr.success('Saved successfully');
          this.clearCallRecord();

        } else {
          this.toastr.error(res.message || 'Failed to save');
        }
      },
      error: () => this.toastr.error('Error saving client')
    });
  }

  @ViewChild('editor') editor: any;
  clearCallRecord(): void {
    this.callLogsInput.record = undefined;
    this.editor?.quill?.setText('');
    this.callLogsInput = new CallRecordModel();
    this.callLogsInput.date = new Date();
    // Default Type is Call
    this.callLogsInput.typeId = 2
  }

  editCallRecord(callRecord: CallRecordModel): void {
    this.callLogsInput = {
      ...callRecord,
      date: new Date(callRecord.date),
      reminderDate: callRecord.reminderDate ? new Date(callRecord.reminderDate) : undefined,
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
        }
      });
    }
  }

  getCallLogs(): void {
    const params = {
      leadsId: this.leadId, // replace with actual clientId
      managerId: 0,  // optional or default
      consultantId: 0, // optional or default
    };

    this.apiService.getDataById('Admin/GetCallRecords', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.callLogsList = res.data;
          this.searchCallLogs = Object.keys(this.callLogsList[0]);

        } else {
          this.toastr.error('Failed to load call logs');
        }
      },
      error: () => this.toastr.error('Error fetching call logs')
    });
  }

}
