import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LeadListModel, LeadModel } from '../../../models/lead/lead-model';
import { ApiService } from '../../../services/api.service';
import { LeadMappingService } from '../mapper/lead-mapper';

@Component({
  selector: 'app-my-leads',
  templateUrl: './my-leads.component.html',
  styleUrl: './my-leads.component.css'
})
export class MyLeadsComponent {

  leadsList: LeadListModel[] = [];
  searchFields: string[] = [];

  constructor(private apiService: ApiService,
    private router: Router,
    private leadMappingService: LeadMappingService
  ) { }

  ngOnInit(): void {
    this.getLeads();
  }

  getLeads() {
    this.apiService.getData('Lead/DetailedLeadsListGet').subscribe({
      next: (response) => {
        this.leadsList = response?.data || [];
        this.searchFields = Object.keys(this.leadsList);
      },
      error: (err) => {
        console.error('Error loading leads list', err);
      }
    });
  }

  editLead(lead: LeadListModel) {
    const leadModel = this.leadMappingService.mapToLeadModel(lead);
    this.router.navigate(['/add-leads'], { state: { lead: leadModel } });
  }

  deleteLead(id: number) {
    if (confirm('Are you sure you want to delete this lead?')) {
      this.apiService.deleteData('Lead/LeadDelete', { id }).subscribe({
        next: (response) => {
          console.log('Lead deleted successfully');
          this.getLeads();
        },
        error: (err) => {
          console.error('Error deleting lead', err);
        }
      });
    }
  }


}
