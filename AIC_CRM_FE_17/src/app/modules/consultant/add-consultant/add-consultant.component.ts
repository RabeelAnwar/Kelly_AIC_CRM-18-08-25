import { Component } from '@angular/core';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Country, State, City } from 'country-state-city';
import { DropdownItem } from '../../../models/common/common';
import { Location } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-add-consultant',
  templateUrl: './add-consultant.component.html',
  styleUrl: './add-consultant.component.css'
})
export class AddConsultantComponent {

  consultantInput: ConsultantModel = new ConsultantModel();
  editMode: boolean = false;

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];
  usersList: DropdownItem[] = [];

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private auth: AuthService,
    private ckConfig: CkeditorConfigService
  ) { }

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  visaStatus: any[] = [
    { name: 'USC' },
    { name: 'GC' },
    { name: 'TN' },
    { name: 'H1' },
    { name: 'EAD' },
    { name: 'Other' },
  ]
  resumePath: string | null = null; // store old path for fallback use
  ngOnInit(): void {

    this.resetForm();

    const state = this.location.getState() as { consultant?: ConsultantModel };
    if (state.consultant) {
      this.consultantInput = state.consultant;

      if (state.consultant.id > 0) {
        this.editMode = true;
        this.resumePath = state.consultant.resume ?? null; // ✅ store old resume path

        this.addressTrigger();
      }
    }

    this.getAllUsers();
    this.getCountries();
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
      this.consultantInput.country = '';
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

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.consultantInput.resumeFile = input.files[0];
    }
  }

  saveConsultant(): void {
    debugger;
    if (!this.consultantInput.firstName || this.consultantInput.firstName.trim() === '') {
      this.toastr.warning('First Name is required');
      return;
    }

    if (!this.consultantInput.lastName || this.consultantInput.lastName.trim() === '') {
      this.toastr.warning('Last Name is required');
      return;
    }

    const formData = new FormData();
    const model = this.consultantInput as any; // Type assertion to bypass TS7053

    for (const key in model) {
      if (model.hasOwnProperty(key) && model[key] !== undefined && model[key] !== null) {
        if (key === 'resumeFile' && model[key] instanceof File) {
          formData.append('ResumeFile', model[key]); // backend expects ResumeFile
        } else {
          formData.append(key, model[key]);
        }
      }
    }
    if (!this.consultantInput.resumeFile && this.resumePath) {
      formData.append('Resume', this.resumePath); // ✅ send existing file path
    }
    this.apiService.saveFormData('Consultant/ConsultantAddUpdate', formData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('Consultant saved successfully');
          this.router.navigate(['/consultant-dashboard', res.data.id]);

          // if (this.editMode) {
          //   this.router.navigate(['/consultant-directory']);
          // }
          this.resetForm();
        } else {
          this.toastr.error(res.message || 'Failed to save consultant');
        }
      },
      error: () => this.toastr.error('Error saving consultant')
    });
  }

  resetForm(): void {
    this.consultantInput = new ConsultantModel();
    this.consultantInput.assignedToId = this.auth.getUserId();
    this.editMode = false;
  }

  onCancel(): void {
    if (this.editMode) {
      this.router.navigate(['/consultant-directory']);
    } else {
      this.router.navigate(['/home']);
    }
  }


  addressTrigger(): void {
    if (this.consultantInput.country) {
      this.getStates(this.consultantInput.country);

      setTimeout(() => {
        if (this.consultantInput.state) {
          this.getCities(this.consultantInput.state);
        }
      }, 200);
    }
  }
  getFileName(path: string): string {
    return path.split('/').pop() ?? '';
  }


}

