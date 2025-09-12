import { Component } from '@angular/core';
import { ClientModel } from '../../../models/client/client-model';
import { Router } from '@angular/router';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';
import { ConsultantInterviewProcessModal } from '../../../models/consultant/interview-process-model';
import { ConsultantActivityModal } from '../../../models/consultant/consultant-activity-model';
import { DropdownItem } from '../../../models/common/common';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-req-consultant-interview-process',
  templateUrl: './req-consultant-interview-process.component.html',
  styleUrls: ['./req-consultant-interview-process.component.css'],
  //styleUrl: './req-consultant-interview-process.component.css',
})
export class ReqConsultantInterviewProcessComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private ckConfig: CkeditorConfigService
  ) {}

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  usersList: DropdownItem[] = [];
  empType: any[] = [
    { name: 'Full Time' },
    { name: 'Corp to Corp' },
    { name: 'W2' },
  ];

  clientData: ClientModel = new ClientModel();
  managerData: ClientManagerModel = new ClientManagerModel();
  requisitionData: RequisitionModel = new RequisitionModel();
  interviewData: ConsultantActivityModal = new ConsultantActivityModal();
  interviewProcessInput: ConsultantInterviewProcessModal =
    new ConsultantInterviewProcessModal();
  activityConsultants: any[] = [];
  activityConsultantsUpdate: ConsultantActivityModal[] = [];
  consultantActivityId = 0;
  consultantName = '';
  consultantId = 0;
  searchFields: string[] = [];

  scheduleInterviewDate: boolean = false;
  scheduleInterviewNotes: boolean = false;

  ngOnInit(): void {
    localStorage.setItem;
    const activityIdStr = localStorage.getItem('activityId');
    if (activityIdStr !== null) {
      this.consultantActivityId = activityIdStr
        ? parseInt(activityIdStr, 10)
        : 0;
      console.log('Parsed activityId:', this.consultantActivityId);
    }

    const state = this.location.getState() as {
      requisition?: RequisitionModel;
      processId?: number;
    };
    if (state.requisition) {
      this.requisitionData = state.requisition;

      this.interviewData.clientId = this.requisitionData.clientId;
      this.interviewData.managerId = this.requisitionData.managerId;
      this.interviewData.requisitionId = this.requisitionData.id;
    }

    this.getAllUsers();
    this.getSingleClient();
    this.getClientManager();
    // if (this.consultantActivityId > 0) {
    //   this.getInterviewProcessConsultantsList();
    // } else {
    //   console.warn(
    //     'No valid consultantActivityId found, skipping consultant list call'
    //   );
    // }
    this.getInterviewProcessConsultantsList();
    this.defaultValues();

    if (state.processId !== undefined && state.processId > 0) {
      this.getInterviewProcessDetails(state.processId);
    }
  }

  updateConsultantActivity(data: ConsultantActivityModal): void {
    this.apiService
      .saveData('Consultant/ConsultantActivityAddUpdate', data)
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            if (res.data.id > 0) {
              this.getInterviewProcessConsultantsList();
            }
            this.toastr.success('Consultant activity saved successfully');
          } else {
            this.toastr.error(
              res.message || 'Failed to save consultant activity'
            );
          }
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error saving consultant activity');
        },
      });
  }

  getInterviewProcessConsultantsList(): void {
    console.log(this.requisitionData);
    this.apiService
      .getData('Consultant/InterviewProcessConsultantsList', {
        requisitionId: this.requisitionData.id,
      })
      .subscribe({
        next: (response) => {
          console.log('API raw response', response); // <-- add this
          if (response.succeeded) {
            const dropdownItems: any[] = [];
            response?.data.forEach((i: any) => {
              dropdownItems.push({
                id: i.consultantId,
                name: i.consultantName,
                consultantActivityId: i.id,
              });
            });
            this.activityConsultants = dropdownItems;

            console.log('Dropdown items', this.activityConsultants); // <-- add this
            if (this.consultantActivityId > 0) {
              this.activityConsultantsUpdate = response?.data?.filter(
                (x: any) => x.id === this.consultantActivityId
              );
              this.searchFields = Object.keys(
                this.activityConsultantsUpdate[0]
              );
              this.consultantName =
                this.activityConsultantsUpdate[0].consultantName!;
              this.consultantId =
                this.activityConsultantsUpdate[0].consultantId;
            }
          } else {
            this.toastr.error('Failed to load');
          }
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading data');
        },
      });
  }

  getInterviewProcessDetails(processId: number): void {
    this.apiService
      .getDataById('Consultant/InterviewProcessDetails', {
        interviewProcessId: processId,
      })
      .subscribe({
        next: (response) => {
          if (response.succeeded) {
            this.interviewProcessInput =
              response?.data || new ConsultantInterviewProcessModal();
            this.interviewProcessInput = {
              ...this.interviewProcessInput,
              date: new Date(this.interviewProcessInput.date),
              expectedStartDate: this.interviewProcessInput.expectedStartDate
                ? new Date(this.interviewProcessInput.expectedStartDate)
                : undefined,
              endDate: this.interviewProcessInput.endDate
                ? new Date(this.interviewProcessInput.endDate)
                : undefined,
            };
            console.log('interviewProcessInput', this.interviewProcessInput);
          }
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading data');
        },
      });
  }

  getSingleClient(): void {
    this.apiService
      .getDataById('Client/SingleClientGet', {
        id: this.interviewData.clientId,
      })
      .subscribe({
        next: (response) => {
          this.clientData = response?.data || new ClientModel();
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading client data');
        },
      });
  }

  getClientManager(): void {
    this.apiService
      .getDataById('Client/ClientManagerGet', {
        id: this.interviewData.managerId,
      })
      .subscribe({
        next: (response) => {
          this.managerData = response?.data || new ClientManagerModel();
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading client data');
        },
      });
  }

  onDropdownChange(event: any) {
    if (event) {
      const consultantActivityId = this.activityConsultants.find(
        (item: any) => item.id === event.value
      ).consultantActivityId;
      this.apiService
        .getDataById('Consultant/InterviewProcessConsultantSingle', {
          id: consultantActivityId,
        })
        .subscribe({
          next: (response) => {
            if (response.succeeded) {
              this.activityConsultantsUpdate = [];
              this.activityConsultantsUpdate.push(response?.data);

              this.searchFields = Object.keys(
                this.activityConsultantsUpdate[0]
              );

              this.consultantName =
                this.activityConsultantsUpdate[0].consultantName!;
              this.consultantId =
                this.activityConsultantsUpdate[0].consultantId;
              this.consultantActivityId = this.activityConsultantsUpdate[0].id;
            } else {
              this.toastr.error('Failed to load');
            }
          },
          error: (err) => {
            this.toastr.error(err?.message || 'Error loading data');
          },
        });
    }
  }

  getAllUsers(): void {
    this.apiService.getData('Admin/UsersListGet').subscribe({
      next: (res) => {
        if (res.succeeded) {
          const dropdownItems: DropdownItem[] = [];
          res?.data.forEach((i: any) => {
            dropdownItems.push({
              id: i.id,
              name: i.firstName + ' ' + i.lastName,
            });
          });
          this.usersList = dropdownItems;
        } else {
          this.toastr.error('Failed to load users');
        }
      },
      error: () => this.toastr.error('Error fetching users'),
    });
  }

  clientDashboard() {
    this.router.navigate(['/client-dashboard', this.clientData.id]);
  }

  goToManagerDashboard() {
    this.router.navigate(['/ManagerDashboard', this.managerData.id]);
  }

  requisitionDashboard() {
    this.router.navigate(['/RequisitionDashboard', this.requisitionData.id]);
  }

  consultantDashboard(id: number): void {
    this.router.navigate(['/consultant-dashboard', id]);
  }

  saveInterviewProcess() {
    // if (!this.interviewProcessInput.date) {
    //   this.toastr.warning('Date is required');
    //   return;
    // }
    this.interviewProcessInput.consultantActivityId = this.consultantActivityId;
    // this.interviewProcessInput.notesDetail =
    //   this.interviewProcessInput.notesDetail;
    if (
      this.interviewProcessInput.notesDetail &&
      this.interviewProcessInput.notesDetail.trim() !== ''
    ) {
      // agar notesDetail me data h to usko notes me assign kro
      this.interviewProcessInput.notes = this.interviewProcessInput.notesDetail;
    } else if (
      this.interviewProcessInput.notes &&
      this.interviewProcessInput.notes.trim() !== ''
    ) {
      // agar notesDetail empty h aur notes me data h to wahi save kro
      this.interviewProcessInput.notes = this.interviewProcessInput.notes;
    } else {
      // dono empty h to null ya empty string save kr do
      this.interviewProcessInput.notes = '';
    }
    //
    console.log('interviewProcessInput', this.interviewProcessInput);

    if (
      this.interviewProcessInput.startCandidate &&
      this.activityConsultantsUpdate.length > 0
    ) {
      const activityUpdate = this.activityConsultantsUpdate[0];
      activityUpdate.billRate = this.interviewProcessInput.billRate;
      activityUpdate.payRate = this.interviewProcessInput.hourlyRate;
      this.updateConsultantActivity(activityUpdate);
    }

    this.apiService
      .saveData(
        'Consultant/InterviewProcessAddUpdate',
        this.interviewProcessInput
      )
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            if (res.data.id > 0) {
              if (res.data.salary > 0 || res.data.hourlyRate > 0) {
                this.requisitionDashboard();
              }
              this.getInterviewProcessConsultantsList();
            }
            this.toastr.success('Saved successfully');
            // Clear the notes here after successful save
            this.interviewProcessInput.notes = '';
            this.router.navigate(['/RequisitionDashboard']);
          } else {
            this.toastr.error(res.message || 'Failed to Save');
          }
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error saving');
        },
      });
  }

  defaultValues() {
    const today = new Date();
    today.setHours(12, 0, 0, 0); // Set time to 12:00:00.000 PM
    this.interviewProcessInput.date = today;

    this.interviewProcessInput.expectedStartDate = new Date();
    this.interviewProcessInput.endDate = new Date();
    this.interviewProcessInput.expenses = 0;
    this.interviewProcessInput.billRate = 0;
    this.interviewProcessInput.markup = 0.0;
  }

  onStartCandidateChange(event: any) {
    this.interviewProcessInput.startCandidate = event.target.checked;

    this.scheduleInterviewDate =
      this.interviewProcessInput.startCandidate || false;
    this.scheduleInterviewNotes =
      this.interviewProcessInput.startCandidate || false;
  }

  hasSelectedStatus(statusList: string[]): boolean {
    return statusList.includes('Selected');
  }

  consultantActivity() {
    this.router.navigate(['/ConsultantActivity'], {
      state: { requisition: this.requisitionData },
    });
  }

  // Flags to track original bill rate
  flgBillRateSAL = true;
  flgBillRateHR = true;
  flgBillRateEXP = true;
  flgBillRateVOP = true;
  flgBillRate = true;

  finalBillRate: number = 0;
  billRateCalc: number = 0;
  disabledHourlyRate: boolean = false;

  ///////////////////////// SALARY ///////////////////////////
  changeSalary(salary: number): void {
    if (salary > 0) {
      this.disabledHourlyRate = true;
      this.interviewProcessInput.hourlyRate = 0;

      const hourly = parseFloat((salary / 2080).toFixed(2));
      this.interviewProcessInput.hourlyRate = hourly;
      const benefit = hourly * 1.25;

      // Corp to Corp Loaded Rate Condition
      if (this.interviewProcessInput.employmentType != 'Corp to Corp') {
        this.interviewProcessInput.loadedRate = +(
          benefit + +this.interviewProcessInput.expenses
        ).toFixed(2);
      } else {
        this.interviewProcessInput.loadedRate =
          this.interviewProcessInput.hourlyRate;
      }

      if (this.flgBillRateSAL) {
        this.finalBillRate = this.interviewProcessInput.billRate;
        this.flgBillRateSAL = false;
      } else {
        this.finalBillRate = 0;
        this.flgBillRateSAL = true;
      }

      this.interviewProcessInput.billRate = 0;
      this.interviewProcessInput.vop = 0;
      this.interviewProcessInput.markup = 0;
    } else {
      this.disabledHourlyRate = false;
      this.interviewProcessInput.hourlyRate = 0;
      this.interviewProcessInput.loadedRate = 0;
    }
  }

  ///////////////////////// HOURLY RATE ///////////////////////////
  changeHourlyRate(hourlyRate: number): void {
    const benefit =
      this.interviewProcessInput.salary > 0
        ? hourlyRate * 1.25
        : hourlyRate * 1.1;

    // Corp to Corp Loaded Rate Condition
    if (this.interviewProcessInput.employmentType != 'Corp to Corp') {
      this.interviewProcessInput.loadedRate = +(
        benefit + +this.interviewProcessInput.expenses
      ).toFixed(2);
    } else {
      this.interviewProcessInput.loadedRate =
        this.interviewProcessInput.hourlyRate;
    }

    if (this.flgBillRateHR) {
      this.finalBillRate = this.interviewProcessInput.billRate;
      this.flgBillRateHR = false;
    }

    this.interviewProcessInput.billRate = 0;
    this.interviewProcessInput.vop = 0;
    this.interviewProcessInput.markup = 0;
  }

  ///////////////////////// EXPENSES ///////////////////////////
  changeExpenses(expense: number): void {
    if (this.flgBillRateEXP) {
      this.finalBillRate = this.interviewProcessInput.billRate;
      this.flgBillRateEXP = false;
    }

    const hourlyRate = this.interviewProcessInput.hourlyRate;
    const benefit =
      this.interviewProcessInput.salary > 0
        ? hourlyRate * 1.25
        : hourlyRate * 1.1;

    // Corp to Corp Loaded Rate Condition
    if (this.interviewProcessInput.employmentType != 'Corp to Corp') {
      this.interviewProcessInput.loadedRate = +(benefit + +expense).toFixed(2);
    } else {
      this.interviewProcessInput.loadedRate =
        this.interviewProcessInput.hourlyRate;
    }

    this.interviewProcessInput.billRate = 0;
    this.interviewProcessInput.vop = 0;
    this.interviewProcessInput.markup = 0;
  }

  ///////////////////////// VOP ///////////////////////////
  changeVOP(vop: number): void {
    const input = this.interviewProcessInput;

    if (this.flgBillRateVOP) {
      this.finalBillRate = input.billRate;
      this.flgBillRateVOP = false;
    }

    if (vop > 0) {
      const vopDecimal = parseFloat((vop / 100).toFixed(2));
      this.billRateCalc = +(this.finalBillRate * vopDecimal).toFixed(2);
      if (this.billRateCalc > 0) {
        input.billRate = this.billRateCalc;
      }
    } else if (this.finalBillRate > 0) {
      input.billRate = this.finalBillRate;
    }

    if (this.finalBillRate > input.billRate) {
      input.billRate = +(this.finalBillRate - input.billRate).toFixed(2);
    }

    if (input.billRate > input.hourlyRate) {
      input.markup = +(
        ((input.billRate - input.hourlyRate) / input.hourlyRate) *
        100
      ).toFixed(2);
    }
  }

  ///////////////////////// BILL RATE ///////////////////////////
  changeBillRate(billRate: number): void {
    const input = this.interviewProcessInput;

    if (this.flgBillRate) {
      this.finalBillRate = billRate;
      this.flgBillRate = false;
    } else {
      this.finalBillRate = 0;
      this.flgBillRate = true;
    }

    input.vop = 0;
    input.markup = 0;

    if (this.finalBillRate > billRate) {
      billRate = +(this.finalBillRate - billRate).toFixed(2);
    }

    input.markup = +(
      ((billRate - input.hourlyRate) / input.hourlyRate) *
      100
    ).toFixed(2);
  }
}
