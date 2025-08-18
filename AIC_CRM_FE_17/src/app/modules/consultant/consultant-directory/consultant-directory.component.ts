import { Component, ViewChild } from '@angular/core';
import { ConsultantModel } from '../../../models/consultant/consultant-model';
import { CustomToastService } from '../../../services/custom-toast.service';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { environment } from '../../../../environment/environmemt';
import { FileDownload } from '../../../services/file.download';
import { ReportsExport } from '../../../services/export-report';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-consultant-directory',
  templateUrl: './consultant-directory.component.html',
  styleUrl: './consultant-directory.component.css'
})
export class ConsultantDirectoryComponent {

  fildteredConsultantsIds: number[] = [];

  consultantsList: ConsultantModel[] = [];
  searchFields: string[] = [];
  inputSearch?: string;

  constructor(
    private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,
    private fileDownload: FileDownload,
    private exportrpt: ReportsExport,
  ) { }

  ngOnInit(): void {
    this.getConsultants();
  }

  // onngdestroy(){
  //   this.getConsultants.un
  // }
  getConsultants() {
    this.apiService.getData('Consultant/ConsultantsListGet').subscribe({
      next: (response) => {

        console.log('list', response)

        if (response.data.length > 0) {
debugger;
          this.consultantsList = response?.data || [];
          this.consultantsList.forEach(i => {

            if (i.resume) {
              i.resume = `${environment.basePath}/` + i.resume;
            }

            const address1 = i.address1 ? this.stripHtml(i.address1).trim() : '';
            const country = i.country || '';
            const state = i.state || '';
            const city = i.city || '';
            const zip = i.zipCode || '';

            i.completeAddress = [address1, country, state, city, zip]
              .filter(Boolean)
              .join(', ');
          });

          this.consultantsList = (response?.data || []).sort((a: any, b: any) => {
            const lastNameA = a.lastName?.toLowerCase() || "";
            const lastNameB = b.lastName?.toLowerCase() || "";
            return lastNameA.localeCompare(lastNameB);
          });
          
          this.searchFields = response.data.length ? Object.keys(response.data[0]) : [];
          console.log(this.consultantsList);
        }

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading consultants list');
      }
    });
  }

  stripHtml(htmlString: any) {
    const temp = document.createElement('div');
    temp.innerHTML = htmlString;
    return temp.textContent || temp.innerText || '';
  }

  downloadFile(fileUrl: string) {
    this.fileDownload.downloadFileByUrl(fileUrl);
  }

  consultantDashboard(id: number) {
    this.router.navigate(['/consultant-dashboard', id], {
      state: { resumeSearchHighLight: this.inputSearch, fildteredConsultantsIds: this.fildteredConsultantsIds }

    });
  }

  editConsultant(consultant: ConsultantModel) {

    this.router.navigate(['/add-consultant'], {
      state: { consultant }
    });

  }

  deleteConsultant(id: number) {
    if (confirm('Are you sure you want to delete this consultant?')) {
      this.apiService.deleteData('Consultant/ConsultantDelete', { id }).subscribe({
        next: () => {
          this.toastr.success('Consultant deleted successfully');
          this.getConsultants();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting consultant');
        }
      });
    }
  }

  exportToWord() {
    this.exportrpt.exportConsultantsListToWord(this.consultantsList);
  }


  onTableFilter(event: any) {
    this.fildteredConsultantsIds = (event.filteredValue || []).map((row: any) => row.id);
    console.log('Filtered Consultant IDs:', this.fildteredConsultantsIds);
  }


}
