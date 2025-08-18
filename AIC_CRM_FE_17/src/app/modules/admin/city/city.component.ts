import { Component } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { CityModel } from '../../../models/common/common';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-city',
  templateUrl: './city.component.html',
  styleUrl: './city.component.css'
})
export class CityComponent {

  cityList: SelectItem[] = [];
  cityModel: CityModel = new CityModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getCities();
    this.cityModel.id = 0;
  }

  getCities(): void {
    this.apiService.getData('Admin/CityGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.cityList = res.data;
        } else {
          this.toastr.error('Failed to load cities');
        }
      },
      error: () => this.toastr.error('Error fetching cities')
    });
  }

  saveCity(): void {
    const params = {
      Id: this.cityModel.id.toString(),
      name: this.cityModel.name
    };

    this.apiService.save('Admin/CityAddUpdate', params, {}).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('City saved successfully');
          this.getCities();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving city')
    });
  }

  deleteCity(id: number): void {
    const params = { Id: id.toString() };

    this.apiService.deleteData('Admin/CityDelete', params).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('City deleted successfully');
          this.getCities();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting city')
    });
  }

  editCity(city: CityModel): void {
    this.cityModel = { ...city };
  }

  clearInput() {
    this.cityModel = new CityModel();
    this.cityModel.id = 0;
  }

}
