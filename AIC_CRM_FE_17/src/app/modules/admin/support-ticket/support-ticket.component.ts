import { Component, OnInit, OnDestroy } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SupportTicketModel } from '../../../models/admin/support-ticket-model';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-support-ticket',
  templateUrl: './support-ticket.component.html',
  styleUrl: './support-ticket.component.css'
})
export class SupportTicketComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private ckConfig: CkeditorConfigService
  ) { }

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  private intervalSubscription: Subscription | undefined;

  input: SupportTicketModel = new SupportTicketModel();

  currentTime: Date = new Date();

  type: any[] = [
    { name: 'Technical' },
    { name: 'Operational' },
    { name: 'Other' },
  ];

  status: any[] = [
    { name: 'Open' },
    { name: 'OnHold' },
    { name: 'InProgress' },
    { name: 'Closed' },
  ];

  priority: any[] = [
    { name: 'High' },
    { name: 'Normal' },
    { name: 'Low' },
  ];

  ngOnInit() {
    this.intervalSubscription = interval(1000).subscribe(() => {
      this.currentTime = new Date();
    });
  }

  ngOnDestroy() {
    if (this.intervalSubscription) {
      this.intervalSubscription.unsubscribe();
    }
  }

  save() {

    if (!this.input.type || this.input.type.trim() === '') {
      this.toastr.warning('Type is required');
      return;
    }

    if (!this.input.status || this.input.status.trim() === '') {
      this.toastr.warning('Status is required');
      return;
    }

    if (!this.input.priority || this.input.priority.trim() === '') {
      this.toastr.warning('Priority is required');
      return;
    }

    if (!this.input.subject || this.input.subject.trim() === '') {
      this.toastr.warning('Subject is required');
      return;
    }

    const formData = new FormData();
    const model = this.input as any; // Type assertion to bypass TS7053

    for (const key in model) {
      if (model.hasOwnProperty(key) && model[key] !== undefined && model[key] !== null) {
        if (key === 'documentFile' && model[key] instanceof File) {
          formData.append('DocumentFile', model[key]); // backend expects ResumeFile
        } else {
          formData.append(key, model[key]);
        }
      }
    }
    this.apiService.saveFormData('Admin/SupportTicketAddUpdate', formData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Ticked saved successfully');
          this.input = new SupportTicketModel();

        } else {
          this.toastr.error(res.message || 'Failed to save ticked');
        }
      },
      error: () => this.toastr.error('Error saving ticked')
    });
  }


}

