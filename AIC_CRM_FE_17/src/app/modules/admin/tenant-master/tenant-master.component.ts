import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { TenantModel } from '../../../models/tenant/tenant-model';
import { City, Country, State } from 'country-state-city';

@Component({
  selector: 'app-tenant-master',
  templateUrl: './tenant-master.component.html',
  styleUrl: './tenant-master.component.css'
})
export class TenantMasterComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) { }

  company: TenantModel = new TenantModel();
  allCompanies: TenantModel[] = [];
  searchFields: string[] = [];

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];

  ngOnInit(): void {
    this.getCountries();
    this.getAllCompanies();
  }

  getCountries(): void {
    this.countries = [
      { name: 'United States' },
      { name: 'India' },
      { name: 'Pakistan' },
      { name: 'Mexico' },
      { name: 'Brazil' },
      { name: 'Other' }
    ];
  }

  onCountryChange(name?: any) {

    if (name === 'Other') {
      this.company.country = '';
      this.countries = Country.getAllCountries();
      this.countries.sort((a, b) => a.name.localeCompare(b.name));
      console.log(this.countries);
    } else {
      this.getStates(name);
    }
  }

  getStates(countryName?: string): void {

    if (countryName) {

      const allCountries = Country.getAllCountries();
      const countryNames = allCountries.sort((a, b) => a.name.localeCompare(b.name));

      const country = countryNames.find((c) => c.name === countryName);
      if (country) {
        this.states = State.getStatesOfCountry(country.isoCode);
        this.states.sort((a, b) => a.name.localeCompare(b.name));
      }
    }
    console.log(this.states);
  }

  getCities(stateName?: string): void {

    if (stateName) {
      const state = this.states.find((s) => s.name === stateName);
      if (state) {
        this.cities = City.getCitiesOfState(state.countryCode, state.isoCode);
        this.cities.sort((a, b) => a.name.localeCompare(b.name));
      }
    }
    console.log(this.cities);

  }

  getAllCompanies(): void {
    this.apiService.getData('Admin/CompaniesListGet').subscribe(res => {
      if (res.succeeded) {

        this.allCompanies = res.data;
        this.searchFields = Object.keys(this.company);

      } else {
        this.toastr.error('Failed to load companies');
      }
    });
  }

  saveCompany(): void {

    if (!this.company.companyName || this.company.companyName.trim() === '') {
      this.toastr.warning('Name is required');
      return;
    }

    this.apiService.saveData('Admin/CompanyAddUpdate', this.company).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Company saved successfully');
          this.resetForm();
          this.getAllCompanies();
        } else {
          this.toastr.error('Failed to save company');
        }
      },
      error: () => this.toastr.error('Error saving company')
    });
  }

  deleteCompany(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this tenant?');
    if (!confirmDelete) {
      return;
    }

    this.apiService.deleteData('Admin/CompanyDelete', { companyId: id }).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Company deleted');
          this.getAllCompanies();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting company')
    });
  }

  editCompany(company: any): void {
    this.company = { ...company };

    if (this.company.id > 0) {
      this.addressTrigger();
    }
  }

  addressTrigger(): void {
    if (this.company.country) {
      this.getStates(this.company.country);

      setTimeout(() => {
        if (this.company.state) {
          this.getCities(this.company.state);
        }
      }, 200);
    }
  }

  resetForm(): void {
    this.company = new TenantModel();
  }

  clearInput(): void {
    this.company = new TenantModel();
  }

  onStatusChange(event: any, company: TenantModel) {
    company.activeStatus = event.target.checked;

    this.apiService.saveData('Admin/CompanyAddUpdate', company).subscribe({
      next: (response) => {
        this.getAllCompanies();
        this.toastr.success('updated successfully');
      },
      error: (err) => {
        this.toastr.error(err || 'Failed to save update');
      }
    });
  }
}
