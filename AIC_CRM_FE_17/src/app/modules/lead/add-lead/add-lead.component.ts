import { Component, Input, input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { DropdownItem } from '../../../models/common/common';
import { LeadModel } from '../../../models/lead/lead-model';
import { Location } from '@angular/common';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { ClientModel } from '../../../models/client/client-model';

@Component({
  selector: 'app-add-lead',
  templateUrl: './add-lead.component.html',
  styleUrl: './add-lead.component.css'
})
export class AddLeadComponent {

  constructor(private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location

  ) { }

  editMode: boolean = false;


  categories: any[] = [
    { name: "A" },
    { name: "B" },
    { name: "C" }
  ];

  leadTypes: any[] = [
    { name: "Straight Lead" },
    { name: "Cross Lead" },
  ];

  leadStatus: any[] = [
    { name: "Open" },
    { name: "On-Hold" },
    { name: "Closed" },
  ];

  managers: DropdownItem[] = [];

  departments: DropdownItem[] = [];
  usersList: DropdownItem[] = [];

  // Form fields for lead
  leadModelInput: LeadModel = new LeadModel();
  clientData: ClientModel = new ClientModel();


  ngOnInit(): void {

    ;

    const state = this.location.getState() as { lead?: LeadModel };
    if (state.lead) {

      // if (state.lead.id > 0) {
      //   this.leadModelInput = state.lead;
      // }
      // else {
      //   this.leadModelInput.clientId = state.lead.clientId;
      // }

      if (state.lead) {
        this.leadModelInput = {
          ...state.lead,
          reminderDateTime: state.lead.reminderDateTime ? new Date(state.lead.reminderDateTime) : new Date(),
        };
        this.clientData.id = this.leadModelInput.clientId;

      }

    }
    this.getSingleClient();
    this.getAllUsers();
    this.getManagersByClientId();
    this.getDepartments();
  }


  getAllUsers(): void {
    this.apiService.getData('Admin/UsersListGet').subscribe(res => {
      if (res.succeeded) {

        const dropdownItems: DropdownItem[] = [];

        res?.data.forEach((i: any) => {
          dropdownItems.push({
            id: i.id,
            name: i.firstName + ' ' + i.lastName
          });
        });

        this.usersList = dropdownItems;
        console.log(this.usersList);
      } else {
        this.toastr.error('Failed to load users');
      }
    });
  }


  getManagersByClientId() {
    this.apiService.getDataById('Client/ClientManagerGetByClientId', { id: this.leadModelInput.clientId }).subscribe({
      next: (response) => {

        const dropdownItems: DropdownItem[] = [];

        response?.data.forEach((manager: any) => {
          dropdownItems.push({
            id: manager.id,
            name: manager.firstName + ' ' + manager.lastName
          });
        });

        this.managers = dropdownItems;
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading managers list');
      }
    });
  }

  getDepartments(): void {
    this.apiService.getData('Admin/DepartmentGet').subscribe({
      next: (res: any) => {

        if (res.succeeded) {
          this.departments = res.data.map((department: DropdownItem) => ({
            id: department.id,
            name: department.name
          }));
        } else {
          this.toastr.error('Failed to load departments');
        }
      },
      error: () => this.toastr.error('Error fetching departments')
    });
  }

  getSingleClient() {
    this.apiService.getDataById('Client/SingleClientGet', { id: this.leadModelInput.clientId }).subscribe({
      next: (response) => {
        this.clientData = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  // Save or update lead
  saveLead() {

    if (!this.isLeadValid()) {
      this.toastr.error('Please fill all required fields.');
      return;
    }

    // Call API to save or update the lead
    this.apiService.saveData('Lead/LeadAddUpdate', this.leadModelInput)
      .subscribe((response) => {

        this.toastr.success('Lead saved successfully');
        this.router.navigate(['/all-leads']);
      }, error => {
        console.error(error);
        this.toastr.error('Error saving lead');
      });
  }

  private isLeadValid(): boolean {
    if (!this.leadModelInput.category || this.leadModelInput.category.trim() === '') return false;
    if (!this.leadModelInput.leadType || this.leadModelInput.leadType.trim() === '') return false;
    if (!this.leadModelInput.statusOfLead || this.leadModelInput.statusOfLead.trim() === '') return false;
    if (!this.leadModelInput.departmentId || this.leadModelInput.departmentId === 0) return false;
    if (!this.leadModelInput.managerId || this.leadModelInput.managerId === 0) return false;
    if (!this.leadModelInput.assignedToId || this.leadModelInput.assignedToId.trim() === '') return false;
    return true;
  }

  onCancel() {
    if (this.editMode) {
      this.router.navigate(['/all-leads']);
    } else {
      this.router.navigate(['/home']);
    }
  }

}
