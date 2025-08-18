import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuditModel } from '../../../models/reports/audit-model';
import { DropdownItem } from '../../../models/common/common';

@Component({
  selector: 'app-audit-rpt',
  templateUrl: './audit-rpt.component.html',
  styleUrl: './audit-rpt.component.css'
})
export class AuditRptComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  recentAudits: number = 0;

  usersList: DropdownItem[] = [];
  auditRptList: AuditModel[] = [];
  input: AuditModel = new AuditModel();

  ngOnInit(): void {
    this.getAllUsers();
    this.setToday();
    this.showRpt();
  }


  getAllUsers(): void {
    this.apiService.getData('Admin/UsersListGet').subscribe({
      next: (res) => {
        if (res.succeeded) {
          const dropdownItems: DropdownItem[] = [];
          res?.data.forEach((i: any) => {
            dropdownItems.push({
              id: i.id,
              name: i.firstName + ' ' + i.lastName
            });
          });
          this.usersList = dropdownItems;
        } else {
          this.toastr.error('Failed to load users');
        }
      },
      error: () => this.toastr.error('Error fetching users')
    });
  }

  setToday() {
    const today = new Date();
    this.input.fromDate = new Date(today.setHours(0, 0, 0, 0));
    this.input.toDate = new Date();
  }

  setThisWeek() {
    const today = new Date();
    const firstDay = new Date(today);
    firstDay.setDate(today.getDate() - today.getDay()); // Sunday
    firstDay.setHours(0, 0, 0, 0);

    const lastDay = new Date(firstDay);
    lastDay.setDate(firstDay.getDate() + 6); // Saturday

    this.input.fromDate = firstDay;
    this.input.toDate = new Date(); // Or lastDay if you want to lock it to end of week
  }

  setThisMonth() {
    const today = new Date();
    const firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDay = new Date(today.getFullYear(), today.getMonth() + 1, 0);

    this.input.fromDate = firstDay;
    this.input.toDate = new Date(); // Or lastDay if you want fixed end
  }

  setYesterday() {
    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    this.input.fromDate = new Date(yesterday.setHours(0, 0, 0, 0));
    this.input.toDate = new Date(yesterday.setHours(23, 59, 59, 999));
  }

  setLastWeek() {
    const today = new Date();
    const lastWeekStart = new Date(today);
    lastWeekStart.setDate(today.getDate() - today.getDay() - 7); // Sunday of last week
    lastWeekStart.setHours(0, 0, 0, 0);

    const lastWeekEnd = new Date(lastWeekStart);
    lastWeekEnd.setDate(lastWeekStart.getDate() + 6);
    lastWeekEnd.setHours(23, 59, 59, 999);

    this.input.fromDate = lastWeekStart;
    this.input.toDate = lastWeekEnd;
  }

  setLastMonth() {
    const today = new Date();
    const firstDayLastMonth = new Date(today.getFullYear(), today.getMonth() - 1, 1);
    const lastDayLastMonth = new Date(today.getFullYear(), today.getMonth(), 0);

    this.input.fromDate = firstDayLastMonth;
    this.input.toDate = lastDayLastMonth;
  }

  showRpt() {
    this.apiService.saveData('Report/AuditRptGet', this.input).subscribe(res => {
      if (res.succeeded) {
        this.auditRptList = res.data
        this.recentAudits = this.auditRptList.length;
      } else {
        this.toastr.error('Failed to load users');
      }
    });

  }

}
