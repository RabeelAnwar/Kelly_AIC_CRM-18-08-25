import { Component } from '@angular/core';
import { CustomToastService } from '../../../services/custom-toast.service';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { ConsultantModel } from '../../../models/consultant/consultant-model';

@Component({
  selector: 'app-search-consultant',
  templateUrl: './search-consultant.component.html',
  styleUrl: './search-consultant.component.css'
})
export class SearchConsultantComponent {

  consultantsList: ConsultantModel[] = [];
  fildteredConsultantsIds: number[] = [];


  constructor(
    private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,
  ) { }

  inputSearch?: string;

  ngOnInit(): void {
    this.getConsultants();
  }

  getConsultants() {
    this.apiService.getData('Consultant/ConsultantsListGet').subscribe({
      next: (response) => {

        console.log('list', response)

        if (response.data.length > 0) {
          this.consultantsList = response?.data || [];

          this.consultantsList = (response?.data || []).sort((a: any, b: any) => {
            const lastNameA = a.lastName?.toLowerCase() || "";
            const lastNameB = b.lastName?.toLowerCase() || "";
            return lastNameA.localeCompare(lastNameB);
          });

        }
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading consultants list');
      }
    });
  }

  // consultantDashboard(id: number) {
  //   this.router.navigate(['/consultant-dashboard', id], {
  //     state: { resumeSearchHighLight: this.inputSearch }

  //   });
  // }

  consultantDashboard(id: number) {
    ;
    this.router.navigate(['/consultant-dashboard', id], {
      state: { resumeSearchHighLight: this.inputSearch, fildteredConsultantsIds: this.fildteredConsultantsIds }

    });
  }

  onTableFilter(event: any) {
    this.fildteredConsultantsIds = (event.filteredValue || []).map((row: any) => row.id);
    console.log('Filtered Consultant IDs:', this.fildteredConsultantsIds);
  }

}
