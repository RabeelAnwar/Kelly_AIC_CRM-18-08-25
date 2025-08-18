import { ChangeDetectorRef, Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { City, Country, State } from 'country-state-city';
import { ApiService } from '../../../services/api.service';
import { ClientModel } from '../../../models/client/client-model';
import { DropdownItem } from '../../../models/common/common';
import { Location } from '@angular/common';
import { CkeditorConfigService } from '../../../services/CkeditorConfigService';

@Component({
  selector: 'app-add-client',
  templateUrl: './add-client.component.html',
  styleUrl: './add-client.component.css'
})
export class AddClientComponent {
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
    private location: Location,
    private route: ActivatedRoute,
    private ckConfig: CkeditorConfigService
  ) { }

  public Editor = this.ckConfig.Editor;
  public config = this.ckConfig.config;


  clientInput: ClientModel = new ClientModel();

  countries: any[] = [];
  states: any[] = [];
  cities: any[] = [];
  client: DropdownItem[] = [];


  ngOnInit(): void {

    const state = this.location.getState() as { client?: ClientModel };
    if (state.client) {
      this.clientInput = state.client;

      if (state.client.id > 0) {
        this.addressTrigger();
      }
    }

    this.getClients();
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
      this.clientInput.country = '';
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

  getClients() {
    this.apiService.getData('Client/ClientsListGet').subscribe({
      next: (response) => {
        const dropdownItems: DropdownItem[] = [];

        response?.data.forEach((client: any) => {
          dropdownItems.push({
            id: client.id,
            name: client.clientName
          });
        });

        this.client = dropdownItems;

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }


  saveClient(): void {
    if (!this.clientInput.clientName) {
      this.toastr.warning('Client Name is required');
      return;
    }

    this.apiService.saveData('Client/ClientAddUpdate', this.clientInput).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.getClients();

          this.router.navigate(['/client-dashboard', res.data.id]);
          this.toastr.success('Client saved successfully');

          this.resetForm();

        } else {
          this.toastr.error(res.message || 'Failed to save client');
        }
      },
      error: () => this.toastr.error('Error saving client')
    });
  }

  resetForm(): void {
    this.clientInput = new ClientModel();
  }

  onCancel() {
    this.router.navigate(['/client-directory']);
  }



  addressTrigger(): void {
    if (this.clientInput.country) {
      this.getStates(this.clientInput.country);

      setTimeout(() => {
        if (this.clientInput.state) {
          this.getCities(this.clientInput.state);
        }
      }, 200);
    }
  }


  @ViewChild('dropdownCity') dropdownCity: any;
  clearCityDropdown() {
    setTimeout(() => {
      this.dropdownCity.editableInputViewChild.nativeElement.value = '';
    }, 100);
  }


  @ViewChild('dropdownState') dropdownState: any;
  clearStateDropdown() {
    setTimeout(() => {
      this.dropdownState.editableInputViewChild.nativeElement.value = '';
    }, 100);
    this.clearCityDropdown();
  }



  @ViewChild('address1Editor') address1Editor: any;
  @ViewChild('address2Editor') address2Editor: any;

  @HostListener('document:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    // if (event.key === 'Tab') {
    //   const focusedElement = document.activeElement;
    //   if (focusedElement === this.address1Editor.el.nativeElement) {
    //     event.preventDefault();
    //     // Focus next field (Address 2)
    //     this.address2Editor.el.nativeElement.focus();
    //   }
    // }
  }



}
