import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { UserModel } from '../../../models/user/user-model';
import { City, Country, State } from 'country-state-city';

@Component({
  selector: 'app-user-master',
  templateUrl: './user-master.component.html',
  styleUrl: './user-master.component.css'
})
export class UserMasterComponent {


  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  userInput: UserModel = new UserModel();

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];

  contactTypes: any[] = [];
  usersList: any[] = [];
  searchFields: string[] = [];

  isAdminEdit: boolean = false;

  ngOnInit(): void {
    this.getAllUsers();
    this.getContactTypes();
    this.getCountries();
  }


  getContactTypes(): void {
    this.apiService.getData('Admin/ContactTypeGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.contactTypes = res.data;
          console.log(this.contactTypes);
        } else {
          this.toastr.error('Failed to load contact types');
        }
      },
      error: () => this.toastr.error('Error fetching contact types')
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
      this.userInput.country = '';
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


  getAllUsers(): void {
    this.apiService.getData('Admin/UsersListGet').subscribe(res => {
      if (res.succeeded) {
        this.usersList = res.data;
        this.searchFields = Object.keys(this.usersList);

        console.log(this.usersList);
      } else {
        this.toastr.error('Failed to load users');
      }
    });
  }


  saveUser(): void {
    if (!this.userInput.firstName || !this.userInput.lastName || !this.userInput.userName || !this.userInput.password
      || !this.userInput.contactTypeId || !this.userInput.state || !this.userInput.city || !this.userInput.country) {
      this.toastr.warning('Please fill in all required fields.');
      return;
    }

    this.apiService.saveData('Admin/UserAddUpdate', this.userInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('User saved successfully');
          this.getAllUsers();
          this.resetForm();
        } else {
          this.toastr.error(res.message || 'Failed to save user');
        }
      },
      error: () => this.toastr.error('Error saving user')
    });
  }

  resetForm(): void {
    this.userInput = new UserModel();
  }

  onContactTypeChange(id: number): void {
    const selectedContactType = this.contactTypes.find((type) => type.id === id);
    if (selectedContactType) {
      this.userInput.contactTypeName = selectedContactType.name;
    }
  }

  onStatusChange(event: any, user: UserModel) {
    user.activeStatus = event.target.checked;

    this.apiService.saveData('Admin/UserAddUpdate', user).subscribe({
      next: (response) => {
        this.getAllUsers();
        this.toastr.success('updated successfully');
      },
      error: (err) => {
        this.toastr.error(err || 'Failed to save update');
      }
    });
  }

  editUser(user: any): void {
    this.userInput = { ...user };

    if (this.userInput.userName.toLowerCase().includes('admin')) {
      this.isAdminEdit = true;
    }
    else {
      this.isAdminEdit = false;
    }

    if (this.userInput?.id && this.userInput.id.length > 0) {
      this.addressTrigger();
    }



  }


  addressTrigger(): void {
    if (this.userInput.country) {
      this.getStates(this.userInput.country);

      setTimeout(() => {
        if (this.userInput.state) {
          this.getCities(this.userInput.state);
        }
      }, 200);
    }
  }


  deleteUser(id: number): void {

    const confirmDelete = confirm('Are you sure you want to delete this user?');
    if (!confirmDelete) {
      return;
    }

    this.apiService.deleteData('Admin/UserDelete', { userId: id }).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('User deleted');
          this.getAllUsers();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting User')
    });
  }


  adminActionsDisable(userName: string): boolean {
    var result = userName.toLowerCase().includes('admin');
    return result;
  }
}
