import { Component } from '@angular/core';
import { City, Country, State } from 'country-state-city';
import { ApiService } from '../../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';
import { ClientModel } from '../../../../models/client/client-model';
import { Location } from '@angular/common';
import { DropdownItem } from '../../../../models/common/common';
import { CkeditorConfigService } from '../../../../services/CkeditorConfigService';

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrl: './add-manager.component.css',
})
export class AddManagerComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private ckConfig: CkeditorConfigService
  ) {}

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;

  clientData: ClientModel = new ClientModel();

  managerInput: ClientManagerModel = new ClientManagerModel();
  managerList: ClientManagerModel[] = [];
  managerListDD: DropdownItem[] = [];

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];

  usersList: any[] = [];
  departmentList: any[] = [];

  ngOnInit(): void {
    debugger;
    const state = this.location.getState() as { manager?: ClientManagerModel };
    if (state.manager) {
      this.managerInput = {
        ...state.manager,
      };
      this.clientData.id = this.managerInput.clientId;

      this.addressTrigger();
    }

    this.getSingleClient();
    this.GetWorkUnderManagers();
    this.getDepartments();
    this.getAllUsers();
    this.getCountries();
  }

  getAllUsers(): void {
    this.apiService.getData('Admin/UsersListGet').subscribe((res) => {
      if (res.succeeded) {
        debugger;
        this.usersList = res.data;
        const temp: any[] = [];
        this.usersList.forEach((i) => {
          temp.push({
            ...i,
            displayContactName: i.firstName + ' ' + i.lastName,
          });
        });
        this.usersList = [...temp];
        console.log(this.usersList);
        const target = JSON.parse(localStorage.getItem('userData')!).userId;
        // Default select first user if list is not empty
        this.managerInput.isAssignedToId = this.usersList.find(
          (i) => i.id === target
        ).id ;

        //this.usersList.length > 0 ? this.usersList[0].id : null;
      } else {
        this.toastr.error('Failed to load users');
      }
    });
  }

  getDepartments(): void {
    this.apiService.getData('Admin/DepartmentGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.departmentList = res.data;
        } else {
          this.toastr.error('Failed to load departments');
        }
      },
      error: () => this.toastr.error('Error fetching departments'),
    });
  }

  GetWorkUnderManagers() {
    this.apiService
      .getDataById('Client/ClientManagerGetByClientId', {
        id: this.managerInput.clientId,
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

          this.managerListDD = dropdownItems;
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading clients list');
        },
      });
  }

  getSingleClient() {
    this.apiService
      .getDataById('Client/SingleClientGet', { id: this.managerInput.clientId })
      .subscribe({
        next: (response) => {
          this.clientData = response?.data || [];
        },
        error: (err) => {
          this.toastr.error(err || 'Error loading clients list');
        },
      });
  }

  getCountries(): void {
    this.countries = [
      { name: 'United States' },
      { name: 'India' },
      { name: 'Pakistan' },
      { name: 'Mexico' },
      { name: 'Brazil' },
      { name: 'Other' },
    ];
  }
  onCountryChange(name?: any) {
    if (name === 'Other') {
      this.managerInput.country = '';
      this.countries = Country.getAllCountries();
      this.countries.sort((a, b) => a.name.localeCompare(b.name));
    } else {
      this.getStates(name);
    }
  }
  getStates(countryName?: string): void {
    if (countryName) {
      const allCountries = Country.getAllCountries();
      const countryNames = allCountries.sort((a, b) =>
        a.name.localeCompare(b.name)
      );

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

  goToDashboard(): void {
    this.router.navigate(['/client-dashboard', this.clientData?.id]);
  }

  saveClientManager(): void {
    if (
      !this.managerInput.firstName ||
      this.managerInput.firstName.trim() === ''
    ) {
      this.toastr.warning('First Name is required');
      return;
    }

    if (
      !this.managerInput.lastName ||
      this.managerInput.lastName.trim() === ''
    ) {
      this.toastr.warning('Last Name is required');
      return;
    }

    this.apiService
      .saveData('Client/ClientManagerAddUpdate', this.managerInput)
      .subscribe({
        next: (res) => {
          if (res.succeeded) {
            const state = this.location.getState() as {
              fromClientDashboard?: boolean;
            };

            this.router.navigate(['/ManagerDashboard', res.data.id], {
              state: {
                fromClientDashboard: state.fromClientDashboard || false,
              },
            });
            this.toastr.success('Manager saved successfully');
            // this.ClientManagerGetByClientId();
          } else {
            this.toastr.error(res.message || 'Failed to save manager');
          }
        },
        error: () => this.toastr.error('Error saving manager'),
      });
  }

  addressTrigger(): void {
    if (this.managerInput.country) {
      this.getStates(this.managerInput.country);

      setTimeout(() => {
        if (this.managerInput.state) {
          this.getCities(this.managerInput.state);
        }
      }, 200);
    }
  }
}
