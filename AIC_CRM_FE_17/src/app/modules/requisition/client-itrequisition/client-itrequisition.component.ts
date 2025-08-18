import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Options } from '@angular-slider/ngx-slider';
import { Location } from '@angular/common';
import { ClientModel } from '../../../models/client/client-model';
import { DropdownItem } from '../../../models/common/common';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-client-itrequisition',
  templateUrl: './client-itrequisition.component.html',
  styleUrl: './client-itrequisition.component.css',
})
export class ClientITRequisitionComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private ckConfig: CkeditorConfigService
  ) {}

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  clientData: ClientModel = new ClientModel();
  managers: DropdownItem[] = [];
  usersList: DropdownItem[] = [];
  requisitionInput: RequisitionModel = new RequisitionModel();

  requisitionType: any[] = [{ name: 'Regular' }, { name: 'Proposal' }];

  processes: any[] = [
    { name: 'Phone' },
    { name: 'Video Call' },
    { name: 'In-Person Only' },
    { name: 'Other' },
  ];

  durations: any[] = [
    { name: 'Hours' },
    { name: 'Days' },
    { name: 'Weeks' },
    { name: 'Months' },
    { name: 'Years' },
  ];

  priority: any[] = [{ id: 1 }, { id: 2 }, { id: 3 }];

  codingValue: number = 0;
  codingOptions: Options = {
    ceil: 100,
    step: 5,
    showTicks: true,
    showTicksValues: true,
    animate: false,
    showSelectionBar: true,
  };

  analysisValue: number = 0;
  analysisOptions: Options = {
    ceil: 100,
    step: 5,
    showTicks: true,
    showTicksValues: true,
    animate: false,
    showSelectionBar: true,
  };

  testingValue: number = 0;
  testingOptions: Options = {
    ceil: 100,
    step: 5,
    showTicks: true,
    showTicksValues: true,
    animate: false,
    showSelectionBar: true,
  };

  otherValue: number = 0;
  otherOptions: Options = {
    ceil: 100,
    step: 5,
    showTicks: true,
    showTicksValues: true,
    animate: false,
    showSelectionBar: true,
  };

  ngOnInit(): void {
    // Check for state data passed via navigation
    const state = this.location.getState() as {
      requisition?: RequisitionModel;
    };
    if (state.requisition) {
      this.requisitionInput = {
        ...state.requisition,
        startDate: state.requisition.startDate
          ? new Date(state.requisition.startDate)
          : new Date(),
      };
      this.clientData.id = this.requisitionInput.clientId;

      this.codingValue = this.requisitionInput.codingValue || 0;
      this.analysisValue = this.requisitionInput.analysisValue || 0;
      this.testingValue = this.requisitionInput.testingValue || 0;
      this.otherValue = this.requisitionInput.otherValue || 0;
    }

    this.getManagersByClientId();
    this.getAllUsers();
    this.getSingleClient();
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

  getManagersByClientId(): void {
    this.apiService
      .getDataById('Client/ClientManagerGetByClientId', {
        id: this.clientData.id,
      })
      .subscribe({
        next: (response) => {
          const dropdownItems: DropdownItem[] = [];
          response?.data.forEach((manager: any) => {
            dropdownItems.push({
              id: manager.id,
              name: manager.firstName + ' ' + manager.lastName,
            });
          });
          this.managers = dropdownItems;
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading managers list');
        },
      });
  }

  getSingleClient(): void {
    this.apiService
      .getDataById('Client/SingleClientGet', { id: this.clientData.id })
      .subscribe({
        next: (response) => {
          this.clientData = response?.data || new ClientModel();
          debugger;
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error loading client data');
        },
      });
  }

  saveRequisition(): void {
    // Basic validation
    if (!this.requisitionInput.internalReqCoordinatorId) {
      this.toastr.warning('Internal Req Coordinator is required');
      return;
    }
    if (
      !this.requisitionInput.requisitionType ||
      this.requisitionInput.requisitionType.trim() === ''
    ) {
      this.toastr.warning('Requisition Type is required');
      return;
    }
    if (!this.requisitionInput.priority) {
      this.toastr.warning('PriorityREQUIRED is required');
      return;
    }
    if (
      !this.requisitionInput.jobTitle ||
      this.requisitionInput.jobTitle.trim() === ''
    ) {
      this.toastr.warning('Job Title is required');
      return;
    }
    if (!this.requisitionInput.managerId) {
      this.toastr.warning('Manager is required');
      return;
    }
    if (!this.requisitionInput.salesRepId) {
      this.toastr.warning('Sales Rep is required');
      return;
    }

    if (!this.requisitionInput.recruiterAssignedId) {
      this.toastr.warning('Recruiter Assigned is required');
      return;
    }

    if (!this.requisitionInput.numberOfPositions) {
      this.toastr.warning('Number of positions is required');
      return;
    }

    // Assign slider values to requisitionInput
    this.requisitionInput.codingValue = this.codingValue;
    this.requisitionInput.analysisValue = this.analysisValue;
    this.requisitionInput.testingValue = this.testingValue;
    this.requisitionInput.otherValue = this.otherValue;
    debugger;
    const input = {
      TenantId: this.requisitionInput?.id ,
      Id: this.requisitionInput?.id || 0,
      ClientId: this.requisitionInput?.clientId,
      InternalReqCoordinatorId: this.requisitionInput?.internalReqCoordinatorId, // Required
      RequisitionType: this.requisitionInput?.requisitionType, // Required
      Priority: this.requisitionInput?.priority,// Required
      JobTitle: this.requisitionInput?.jobTitle, // Required
      ClientReqNumber: this.requisitionInput?.clientReqNumber,
      ManagerId: this.requisitionInput?.managerId, // Required
      SalesRepId: this.requisitionInput?.salesRepId, // Required
      RecruiterAssignedId: [], // Required
      Location: this.requisitionInput?.location,
      Duration: this.requisitionInput?.duration,
      DurationTypes: this.requisitionInput?.durationTypes,
      StartDate: this.requisitionInput?.startDate, // Required
      NumberOfPositions: this.requisitionInput?.numberOfPositions, // Required
      Comments: this.requisitionInput?.comments,
      ProjectDepartmentOverview: this.requisitionInput?.projectDepartmentOverview,
      JobDescription: this.requisitionInput?.jobDescription,
      PayRate: this.requisitionInput?.payRate,
      BillRate: this.requisitionInput?.billRate,
      Hours: this.requisitionInput?.hours,
      Overtime: this.requisitionInput?.overtime,
      InterviewProcesses: this.requisitionInput?.interviewProcesses,
      PhoneHireIfOutOfArea: this.requisitionInput?.phoneHireIfOutOfArea,
      ClientMarkup: this.requisitionInput?.clientMarkup,
      BillRateHighestBeforeResumeNotSent: this.requisitionInput?.billRateHighestBeforeResumeNotSent,
      SecondaryContact: this.requisitionInput?.secondaryContact,
      HiringManagerVop: this.requisitionInput?.hiringManagerVop,
      OtherWaysToFillPosition: this.requisitionInput?.otherWaysToFillPosition,
      Notes: this.requisitionInput?.notes,
      Responsibilities: this.requisitionInput?.responsibilities,
      Qualifications:  this.requisitionInput?.qualifications,
      SearchString1: this.requisitionInput?.searchString1,
      SearchString2: this.requisitionInput?.searchString2,
      SearchString3: this.requisitionInput?.searchString3,
      CodingValue: this.requisitionInput.codingValue,
      AnalysisValue: this.requisitionInput.analysisValue,
      TestingValue: this.requisitionInput.testingValue,
      OtherValue: this.requisitionInput.otherValue,

      // Technical Skills Section
      Hardware: this.requisitionInput?.hardware,
      OS: this.requisitionInput?.os,
      Languages: this.requisitionInput?.languages,
      Databases: this.requisitionInput?.databases,
      Protocols: this.requisitionInput?.protocols,
      SoftwareStandards: this.requisitionInput?.softwareStandards,
      Others: this.requisitionInput?.others,
      Status: this.requisitionInput?.status,
    };
    // Save or update requisition
    console.log(input);
    debugger;
    this.apiService
      .saveData('Client/ClientRequisitionAddUpdate', input)
      .subscribe({
        next: (res) => {
          console.log(res);
          debugger
          if (res.succeeded) {
            debugger;
            this.toastr.success('Requisition saved successfully');
            this.router.navigate(['/RequisitionDashboard', res.data.id], {
              state: { requisition: res.data },
            });
          } else {
            this.toastr.error(res.message || 'Failed to save requisition');
          }
        },
        error: (err) => {
          console.log(err);
          debugger;
          this.toastr.error(err?.message || 'Error saving requisition');
        },
      });
  }

  cancel(): void {
    this.requisitionInput = new RequisitionModel();
    this.router.navigate(['/client-dashboard', this.clientData?.id]);
  }
}
