import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { TenantModel } from '../../../models/tenant/tenant-model';
import { Router } from '@angular/router';
import { Country, State, City } from 'country-state-city';

@Component({
  selector: 'app-company-register',
  templateUrl: './company-register.component.html',
  styleUrls: ['./company-register.component.css'],
})
export class CompanyRegisterComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  company: TenantModel = new TenantModel();

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];

  saveButtonText: string = 'Register';
  showIsActive: boolean = false;
  currentUrl: string = '';

  ngOnInit(): void {

    this.currentUrl = this.router.url;
    if (this.currentUrl === '/company-profile') {
      this.showIsActive = true;
      this.saveButtonText = 'Update';
      this.getCurrentCompany();
    }
    else {
      this.company.activeStatus = false;
    }

    this.getCountries();
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

  saveCompany(): void {
    if (!this.company.companyName || this.company.companyName.trim() === '') {
      this.toastr.warning('Name is required');
      return;
    }

    this.apiService.saveData('Admin/CompanyAddUpdate', this.company).subscribe({
      next: (res) => {
        if (res.succeeded) {

          if (this.currentUrl === '/company-profile') {
            this.toastr.success('Company Update successfully');
          }
          else {
            this.router.navigate(['/login']);
            this.toastr.success('Registration Successfull');
          }
        } else {
          this.toastr.error('Failed to save company');
        }
      },
      error: () => this.toastr.error('Error saving company')
    });
  }


  getCurrentCompany(): void {
    this.apiService.getData('Admin/CompanyProfileGet').subscribe(res => {
      if (res.succeeded) {
        this.company = res.data;
        this.addressTrigger();
      } else {
        this.toastr.error('Failed to load company');
      }
    });
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

}
