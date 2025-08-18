import { Component } from '@angular/core';
import { ApiService } from '../../../../services/api.service';
import { Router } from '@angular/router';
import { CustomToastService } from '../../../../services/custom-toast.service';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';

@Component({
  selector: 'app-search-managers',
  templateUrl: './search-managers.component.html',
  styleUrl: './search-managers.component.css'
})
export class SearchManagersComponent {

  managersList: any[] = [];
  searchFields: string[] = [];
  fildteredManagersIds: number[] = [];


  constructor(private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,

  ) { }

  ngOnInit(): void {
    this.getManagers();
  }

  getManagers() {
    this.apiService.getData('Client/ClientManagersListGet').subscribe({
      next: (response) => {
        this.managersList = response?.data || [];

        this.managersList.forEach(i => {
          const address1 = i.address1 ? this.stripHtml(i.address1).trim() : '';
          const country = i.country || '';
          const state = i.state || '';
          const city = i.city || '';
          const zip = i.zipCode || '';
          i.fullName = `${i.firstName || ''} ${i.lastName || ''}`.trim();

          i.completeAddress = [address1, country, state, city, zip]
            .filter(Boolean)
            .join(', ');
        });

        console.log(this.managersList);
        this.searchFields = Object.keys(this.managersList[0]);

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading managers list');
      }
    });
  }
  stripHtml(htmlString: any) {
    const temp = document.createElement('div');
    temp.innerHTML = htmlString;
    return temp.textContent || temp.innerText || '';
  }

  deleteClientManager(id: number) {
    if (confirm('Are you sure you want to delete this manager?')) {
      this.apiService.deleteData('Client/ClientManagerDelete', { id }).subscribe({
        next: (response) => {
          this.toastr.success('Manager deleted successfully');
          this.getManagers();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting manager');
        }
      });
    }
  }

  goToManagerDashboard(manager: ClientManagerModel) {
    this.router.navigate(['/ManagerDashboard', manager?.id], {
      state: { fildteredManagersIds: this.fildteredManagersIds }

    });
  }

  onTableFilter(event: any) {
    this.fildteredManagersIds = (event.filteredValue || []).map((row: any) => row.id);
    console.log('Filtered Manager IDs:', this.fildteredManagersIds);
  }

}
