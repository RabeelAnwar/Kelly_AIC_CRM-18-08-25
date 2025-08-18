import { Component } from '@angular/core';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { ClientModel } from '../../../models/client/client-model';
import { Router } from '@angular/router';
import { ConsultantActivityModal } from '../../../models/consultant/consultant-activity-model';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../../services/api.service';
import { Location } from '@angular/common';
import { DropdownItem } from '../../../models/common/common';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-consultant-activity',
  templateUrl: './consultant-activity.component.html',
  styleUrl: './consultant-activity.component.css'
})
export class ConsultantActivityComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private authService: AuthService
  ) { }

  clientData: ClientModel = new ClientModel();
  consultantData: ConsultantModel = new ConsultantModel();
  managerData: ClientManagerModel = new ClientManagerModel();
  requisitionData: RequisitionModel = new RequisitionModel();
  activityData: ConsultantActivityModal = new ConsultantActivityModal();

  activitySearchedData: ConsultantActivityModal[] = [];
  usersList: DropdownItem[] = [];
  activityConsultantSideList: any[] = [];
  searchString: string = '';
  loggedInUserId: string = '';

  ngOnInit(): void {

    this.loggedInUserId = this.authService.getUserId();

    const state = this.location.getState() as { requisition?: RequisitionModel };
    if (state.requisition) {
      this.requisitionData = state.requisition;

      this.activityData.clientId = this.requisitionData.clientId
      this.activityData.managerId = this.requisitionData.managerId
      this.activityData.requisitionId = this.requisitionData.id
      this.activityData.assignedToId = this.loggedInUserId;
    }

    this.getAllUsers();
    this.getSingleClient();
    this.getClientManager();
    this.getInterviewProcessConsultantsList();

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

  getSingleClient(): void {
    this.apiService.getDataById('Client/SingleClientGet', { id: this.activityData.clientId }).subscribe({
      next: (response) => {
        this.clientData = response?.data || new ClientModel();
      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error loading client data');
      }
    });
  }

  getClientManager(): void {
    this.apiService.getDataById('Client/ClientManagerGet', { id: this.activityData.managerId }).subscribe({
      next: (response) => {
        this.managerData = response?.data || new ClientManagerModel();
      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error loading client data');
      }
    });
  }

  searchConsultantsForActivity() {
    this.apiService.getDataByName('Consultant/SearchConsultantsForActivity', { name: this.searchString }).subscribe({
      next: (res) => {

        if (res.succeeded) {
          this.activitySearchedData = res?.data;

          const consultantIds = new Set(this.activityConsultantSideList.map(item => item.consultantId));
          this.activitySearchedData = this.activitySearchedData.map(item => {
            if (consultantIds.has(item.consultantId)) {
              return { ...item, disabled: true };
            }
            return item;
          });

        } else {
          this.toastr.error('Failed to load users');
        }

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisitions list');
      }
    });
  }

  saveConsultantActivity(data: ConsultantActivityModal): void {

    let saveActivityData: ConsultantActivityModal = new ConsultantActivityModal();

    saveActivityData.id = 0;
    saveActivityData.clientId = this.activityData.clientId;
    saveActivityData.managerId = this.activityData.managerId;
    saveActivityData.consultantId = data.consultantId;
    saveActivityData.requisitionId = this.activityData.requisitionId;
    saveActivityData.assignedToId = this.activityData.assignedToId;

    saveActivityData.consultantName = data.consultantName;
    saveActivityData.billRate = data.billRate;
    saveActivityData.payRate = data.payRate;
    saveActivityData.lastContact = data.lastContact;

    if (!saveActivityData.consultantId || !saveActivityData.assignedToId) {
      this.toastr.warning('Consultant and AssignedTo are required');
      return;
    }

    this.apiService.saveData('Consultant/ConsultantActivityAddUpdate', saveActivityData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          if (res.data.id > 0) {

            data.disabled = true;
            this.getInterviewProcessConsultantsList();

          }
          this.toastr.success('Consultant activity saved successfully');
        } else {
          this.toastr.error(res.message || 'Failed to save consultant activity');
        }
      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error saving consultant activity');
      }
    });
  }



  getInterviewProcessConsultantsList(): void {
    this.apiService.getData('Consultant/InterviewProcessConsultantsList').subscribe({
      next: (response) => {
        if (response.succeeded) {

          this.activityConsultantSideList = response?.data;

        } else {
          this.toastr.error('Failed to load users');
        }

      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error loading client data');
      }
    });
  }

  deleteConsultantActivity(id: number): void {
    debugger;
    if (confirm('Are you sure you want to delete this activity?')) {
      this.apiService.deleteData('Consultant/ConsultantActivityDelete', { id: id }).subscribe({
        next: (res) => {
          this.toastr.success('Deleted successfully');
          this.getInterviewProcessConsultantsList();
        },
        error: (err) => {
          this.toastr.error(err?.message || 'Error deleting activity');
        }
      });
    }
  }



  clientDashboard() {
    this.router.navigate(['/client-dashboard', this.clientData.id]);
  }

  goToManagerDashboard() {
    this.router.navigate(['/ManagerDashboard', this.managerData.id]);
  }

  consultantDashboard(id: number) {
    this.router.navigate(['/consultant-dashboard', id]);
  }

  requisitionDashboard() {
    this.router.navigate(['/RequisitionDashboard', this.requisitionData.id]);
  }


  reqConsultantInterviewProcess() {

    this.router.navigate(['/ReqConsultantInterviewProcess'], {
      state: { requisition: this.requisitionData }
    });
  }

}
