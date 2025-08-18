import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { SelectItem } from 'primeng/api';
import { CountryModel } from '../../../models/common/common';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrl: './country.component.css'
})
export class CountryComponent {

  countryList: CountryModel[] = [];
  countryInput: CountryModel = new CountryModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
    this.getCountries();
    this.countryInput.id = 0; // Initialize ID to 0 for new country
  }

  getCountries() {
    this.apiService.getData('Admin/CountryGet').subscribe((res: any) => {
      if (res.succeeded) {
        this.countryList = res.data;
      } else {
        this.toastr.error('Failed to load countries');
      }
    });
  }

  saveCountry() {
    const params = {
      Id: this.countryInput.id,
      name: this.countryInput.name
    };

    // Using save with query params and an empty body
    this.apiService.save('Admin/CountryAddUpdate', params, {}).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Country saved successfully');
          this.getCountries();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving country')
    });
  }

  deleteCountry(id: number) {
    const params = { Id: id.toString() };

    const confirmDelete = confirm('Are you sure you want to delete this item?');
    if (!confirmDelete) {
      return;
    }

    // Using save (post + query params) as backend expects POST not DELETE
    this.apiService.deleteData('Admin/CountryDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Country deleted successfully');
          this.getCountries();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting country')
    });
  }
  editCountry(country: CountryModel) {
    this.countryInput = { ...country }; // Bind form
  }

  clearInput() {
    this.countryInput = new CountryModel();
    this.countryInput.id = 0; // Reset ID for new country
  }

}
