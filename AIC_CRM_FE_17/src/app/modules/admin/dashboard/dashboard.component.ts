import { Component, ElementRef, AfterViewInit, Renderer2, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LeadModel } from '../../../models/lead/lead-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DashboardModel } from '../../../models/admin/dashboard-model';
import { AuditModel } from '../../../models/reports/audit-model';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'] // fixed typo from "styleUrl" to "styleUrls"
})
export class DashboardComponent implements AfterViewInit,OnInit {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private el: ElementRef,
    private renderer: Renderer2,
    private auth: AuthService,
  ) { }
  private intervalId: any;
   TenantName: string = ''; 
 getTenentName() {
    this.TenantName = this.auth.getTenantName();
    console.log('Tenant Name:', this.TenantName);

  }
  dashboardData: DashboardModel = new DashboardModel();


  //////////////////////////////////////
  recentClientCallList: any[] = [];
  recentConsultantCallList: any[] = [];
  activitiesList: any[] = [];
  recentRequisitionsList: any[] = [];
  leadsList: any[] = [];
  reminderManagerCallsList: any[] = [];
  reminderConsultantCallsList: any[] = [];
  ticketsList: any[] = [];
  recentActivitiesList: any[] = [];


  //////////////////////////////////////
  searchFieldRecentClientCall: string[] = []
  searchFieldRecentConsultantCall: string[] = []
  searchFieldActivities: string[] = []
  searchFieldRecentRequisitions: string[] = []
  searchFieldLeads: string[] = []
  searchFieldReminderManagerCalls: string[] = []
  searchFieldReminderConsultantCalls: string[] = []
  searchFieldTickets: string[] = []


  //////////////////////////////////////
  interviewsList: any[] = [];
  searchFieldInterview: string[] = []


  ngOnInit() {
    this.GetAllDashboardCounts();
    this.getInterviews();
    this.getTenentName();

    this.intervalId = setInterval(() => {
      if (this.storeBoxName && this.storeBoxName.trim() !== '') {
        this.selectBox(this.storeBoxName);
      }

    }, 5000);
  }

  raiseTicket(): void {
    this.router.navigate(['/support-ticket']);
  }


  ngAfterViewInit(): void {
    this.activateListItems();
    this.listenForOutsideClicks();
  }
  private documentClickListener: (() => void) | undefined;

  ngOnDestroy(): void {
    if (this.documentClickListener) {
      this.documentClickListener(); // clean up listener
    }

    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  activateListItems(): void {
    const listItems = this.el.nativeElement.querySelectorAll('.card-body ul li');

    listItems.forEach((item: HTMLElement) => {
      item.addEventListener('click', (event) => {
        event.stopPropagation(); // prevent triggering the document click
        listItems.forEach((li: HTMLElement) => li.classList.remove('active'));
        item.classList.add('active');
      });
    });
  }

  listenForOutsideClicks(): void {
    this.documentClickListener = this.renderer.listen('document', 'click', (event: MouseEvent) => {
      const target = event.target as HTMLElement;

      if (!this.isInsideUlOrLi(target)) {
        const listItems = this.el.nativeElement.querySelectorAll('.card-body ul li');
        listItems.forEach((li: HTMLElement) => li.classList.remove('active'));
      }
    });
  }


  isInsideUlOrLi(element: HTMLElement | null): boolean {
    while (element) {
      if (element.tagName === 'UL' || element.tagName === 'LI') {
        return true;
      }
      element = element.parentElement;
    }
    return false;
  }

  calculatePercentage(billRate: number, payRate: number): string {
    if (isNaN(billRate) || isNaN(payRate) || payRate === null || billRate === null || payRate === 0) {
      return '0';
    }
    const percentage = ((billRate - payRate) / payRate) * 100;
    return percentage.toFixed(2); // you can adjust decimal places as per your need
  }


  GetAllDashboardCounts(): void {
    this.apiService.getData('Admin/GetAllDashboardCounts').subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.dashboardData = res.data;
        } else {
          this.toastr.error('Failed to load data');
        }
      },
      error: () => this.toastr.error('Error fetching data')
    });
  }

  boxes: any = {
    boxRecentClientCall: false,
    boxRecentConsultantCall: false,
    boxActivities: false,
    boxRecentRequisitions: false,
    boxLeads: false,
    boxReminderCalls: false,
    boxTickets: false,
  };

  defaultContent: boolean = true;
  hideBoxesContent() {
    Object.keys(this.boxes).forEach(key => this.boxes[key] = false);
  }

  storeBoxName: any;
  selectBox(boxName: keyof typeof this.boxes) {
    this.hideBoxesContent();
    this.boxes[boxName] = true;
    this.defaultContent = false;
    this.storeBoxName = boxName;
    switch (boxName) {
      case 'boxRecentClientCall':
        this.managerCallRecordsListGet();
        break;
      case 'boxRecentConsultantCall':
        this.consultantCallRecordsListGet();
        break;
      case 'boxActivities':
        this.getActivities();
        break;

      case 'boxRecentRequisitions':
        this.recentRequisitionListGet();
        break;
      case 'boxLeads':
        this.leadsListGet();
        break;
      case 'boxReminderCalls':
        this.managerCallRecordsListGet(true);
        this.consultantCallRecordsListGet(true);
        break;
      case 'boxTickets':
        this.ticketListGet();
        break;
      default:
        // No action needed for other boxes
        break;
    }
  }

  managerCallRecordsListGet(reminder?: boolean): void {

    if (reminder == true) {

      this.apiService.getDataById('Admin/ManagerCallRecordsListGet', { reminder: true }).subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.reminderManagerCallsList = res.data;
            console.log('ManagerCallRecordsListGet', res.data);

            this.searchFieldReminderManagerCalls = res.data.length ? Object.keys(res.data[0]) : [];


          } else {
            this.toastr.error('Failed to load data');
          }
        },
        error: () => this.toastr.error('Error fetching data')
      });

    }
    else {

      this.apiService.getData('Admin/ManagerCallRecordsListGet').subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.recentClientCallList = res.data;
            console.log('ManagerCallRecordsListGet', res.data);
            this.searchFieldRecentClientCall = res.data.length ? Object.keys(res.data[0]) : [];

          } else {
            this.toastr.error('Failed to load data');
          }
        },
        error: () => this.toastr.error('Error fetching data')
      });
    }

  }


  consultantCallRecordsListGet(reminder?: boolean): void {
    if (reminder == true) {

      this.apiService.getDataById('Admin/ConsultantCallRecordsListGet', { reminder: true }).subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.reminderConsultantCallsList = res.data;
            console.log('ConsultantCallRecordsListGet', res.data);
            this.searchFieldReminderConsultantCalls = res.data.length ? Object.keys(res.data[0]) : [];

          } else {
            this.toastr.error('Failed to load data');
          }
        },
        error: () => this.toastr.error('Error fetching data')
      });

    }
    else {

      this.apiService.getData('Admin/ConsultantCallRecordsListGet').subscribe({
        next: (res) => {
          if (res.succeeded) {
            this.recentConsultantCallList = res.data;
            console.log('ConsultantCallRecordsListGet', res.data);
            this.searchFieldRecentConsultantCall = res.data.length ? Object.keys(res.data[0]) : [];

          } else {
            this.toastr.error('Failed to load data');
          }
        },
        error: () => this.toastr.error('Error fetching data')
      });

    }

  }

  recentRequisitionListGet(): void {
    this.apiService.getData('Admin/RecentRequisitionListGet').subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.recentRequisitionsList = res.data;
          console.log('RecentRequisitionListGet', res.data);
          this.searchFieldRecentRequisitions = res.data.length ? Object.keys(res.data[0]) : [];

        } else {
          this.toastr.error('Failed to load data');
        }
      },
      error: () => this.toastr.error('Error fetching data')
    });
  }


  leadsListGet(): void {
    this.apiService.getData('Admin/LeadsListGet').subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.leadsList = res.data;
          console.log('LeadsListGet', res.data);
          this.searchFieldLeads = res.data.length ? Object.keys(res.data[0]) : [];

        } else {
          this.toastr.error('Failed to load data');
        }
      },
      error: () => this.toastr.error('Error fetching data')
    });
  }


  ticketListGet(): void {
    this.apiService.getData('Admin/TicketListGet').subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.ticketsList = res.data;
          console.log('TicketListGet', res.data);
          this.searchFieldTickets = res.data.length ? Object.keys(res.data[0]) : [];

        } else {
          this.toastr.error('Failed to load data');
        }
      },
      error: () => this.toastr.error('Error fetching data')
    });
  }


  getInterviews(): void {
    this.apiService.getData('Admin/GetDashboardInterviewsList').subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.interviewsList = res.data;
          console.log('GetInterviews', res.data);
          this.searchFieldInterview = res.data.length ? Object.keys(res.data[0]) : [];

        } else {
          this.toastr.error('Failed to load data');
        }
      },
      error: () => this.toastr.error('Error fetching data')
    });
  }


  requisitionDashboard(id: number) {
    this.router.navigate(['/RequisitionDashboard', id]);
  }

  goToclientDashboard(id: number) {
    this.router.navigate(['/client-dashboard', id]);
  }

  goToConsultantDashboard(id: number) {
    this.router.navigate(['/consultant-dashboard', id]);
  }


  getActivities() {
    const input = new AuditModel();
    input.fromDate = new Date('2000-01-01');
    input.toDate = new Date();
    this.apiService.saveData('Report/AuditRptGet', input).subscribe(res => {
      if (res.succeeded) {
        this.recentActivitiesList = res.data
      } else {
        this.toastr.error('Failed to load users');
      }
    });
  }


}
