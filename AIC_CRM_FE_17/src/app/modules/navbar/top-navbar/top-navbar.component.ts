import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { AuditModel } from '../../../models/reports/audit-model';

@Component({
  selector: 'app-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrl: './top-navbar.component.css',
})
export class TopNavbarComponent {
  tenantId: any;
  userFullName: string = '';
  TenantName: string = '';  

  notificationsList: AuditModel[] = [];
  input: AuditModel = new AuditModel();

  constructor(
    private auth: AuthService,
    private apiService: ApiService,
    private toastr: ToastrService
  ) {
    this.getTenentId();
    this.getUserFullName();
    this.getTenentName();
  }

  getTenentId() {
    this.tenantId = this.auth.getTenantId();
    console.log('Tenant ID:', this.tenantId);
  }
  getTenentName() {
    this.TenantName = this.auth.getTenantName();
    debugger;
    console.log('Tenant Name:', this.TenantName);

  }
  getUserFullName() {
    this.userFullName = this.auth.getUserFullName();
    console.log('Full Name:', this.userFullName);
  }

  logout() {
    this.auth.logout();
  }

  getNotifications() {
    this.input.fromDate = new Date('2000-01-01');
    this.input.toDate = new Date();

    this.apiService
      .saveData('Report/AuditRptGet', this.input)
      .subscribe((res) => {
        if (res.succeeded) {
          this.notificationsList = res.data;
        } else {
          this.toastr.error('Failed to load');
        }
      });
  }
}
