import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ClientModel } from '../../../models/client/client-model';
import { ApiService } from '../../../services/api.service';
import { CustomToastService } from '../../../services/custom-toast.service';

@Component({
  selector: 'app-client-directory',
  templateUrl: './client-directory.component.html',
  styleUrl: './client-directory.component.css'
})
export class ClientDirectoryComponent {

  clientsList: ClientModel[] = [];
  searchFields: string[] = [];


  constructor(private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,

  ) { }

  ngOnInit(): void {
    this.getClients();
  }

  getClients() {
    this.apiService.getData('Client/ClientsListGet').subscribe({
      next: (response) => {
        this.clientsList = response?.data || [];

        this.searchFields = Object.keys(this.clientsList);

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }


  editClient(client: ClientModel) {
    this.router.navigate(['/add-client'], {
      state: { client }
    });
  }

  deleteClient(id: number) {
    if (confirm('Are you sure you want to delete this client?')) {
      this.apiService.deleteData('Client/ClientDelete', { id }).subscribe({
        next: (response) => {
          this.toastr.success('Client deleted successfully');
          this.getClients();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting client');
        }
      });
    }
  }

    clientDashboard(id: number) {
    this.router.navigate(['/client-dashboard', id]);
  }


  onRegisteredChange(event: any, client: ClientModel) {
    client.registered = event.target.checked;

    this.apiService.saveData('Client/ClientAddUpdate', client).subscribe({
      next: (response) => {
        this.getClients();
        this.toastr.success('updated successfully');
      },
      error: (err) => {
        this.toastr.error(err || 'Failed to save update');
      }
    });
  }

}
