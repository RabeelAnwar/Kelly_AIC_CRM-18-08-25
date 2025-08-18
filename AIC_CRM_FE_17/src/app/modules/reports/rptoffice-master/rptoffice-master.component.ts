import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AllRecruitersModel } from '../../../models/reports/all-recruiters-model';
import { OfficeMasterModel } from '../../../models/reports/office-master-model';

@Component({
  selector: 'app-rptoffice-master',
  templateUrl: './rptoffice-master.component.html',
  styleUrl: './rptoffice-master.component.css'
})
export class RPTOfficeMasterComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  input: OfficeMasterModel = new OfficeMasterModel();
  report: OfficeMasterModel = new OfficeMasterModel();


  ngOnInit(): void {
    this.setToday();
    this.showRpt();
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
    this.apiService.saveData('Report/OfficeMasterRptGet', this.input).subscribe(res => {
      if (res.succeeded) {
        this.report = res.data;
        console.log(res.data);
      } else {
        this.toastr.error('Failed to load users');
      }
    });

  }


  totalBillRate(): number {
    if (!this.report?.requisitionInterviews || !Array.isArray(this.report.requisitionInterviews)) {
      return 0;
    }

    const total = this.report.requisitionInterviews.reduce((sum, interview) => {
      return sum + (typeof interview.billRate === 'number' ? interview.billRate : 0);
    }, 0);

    return Math.round(total * 100) / 100;
  }



  totalHourlyRate(): number {
    if (!this.report?.requisitionInterviews || !Array.isArray(this.report.requisitionInterviews)) {
      return 0;
    }

    const total = this.report.requisitionInterviews.reduce((sum, interview) => {
      return sum + (typeof interview.hourlyRate === 'number' ? interview.hourlyRate : 0);
    }, 0);

    return Math.round(total * 100) / 100;
  }


  totalMarkup(): number {
    if (!this.report?.requisitionInterviews || !Array.isArray(this.report.requisitionInterviews)) {
      return 0;
    }

    const total = this.report.requisitionInterviews.reduce((sum, interview) => {
      const { billRate, hourlyRate } = interview;
      if (typeof billRate === 'number' && typeof hourlyRate === 'number' && hourlyRate !== 0) {
        const markup = ((billRate - hourlyRate) / hourlyRate) * 100;
        return sum + markup;
      }
      return sum;
    }, 0);

    return Math.round(total * 100) / 100;
  }



  requisitionDashboard(id: number) {
    this.router.navigate(['/RequisitionDashboard', id]);
  }

  goToManagerDashboard(id: number) {
    this.router.navigate(['/ManagerDashboard', id]);
  }

  goToclientDashboard(id: number) {
    this.router.navigate(['/client-dashboard', id]);
  }



}
