import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Table } from 'primeng/table';
import { ClientModel } from '../../../models/client/client-model';
import { ApiService } from '../../../services/api.service';
import { CustomToastService } from '../../../services/custom-toast.service';

@Component({
  selector: 'app-search-clients',
  templateUrl: './search-clients.component.html',
  styleUrls: ['./search-clients.component.css'],
})
export class SearchClientsComponent implements OnInit {
  clientsList: ClientModel[] = [];
  @ViewChild('dt') dt: Table | undefined;

  constructor(
    private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService
  ) { }

  ngOnInit(): void {
    this.getClients();
  }

  getClients(): void {
    this.apiService.getData('Client/ClientsListGet').subscribe({
      next: (response) => {
        this.clientsList = response?.data || [];
      },
      error: (err) => {
        this.toastr.error(err.message || 'Error loading clients list');
      },
    });
  }


  clientDashboard(id: number) {
    this.router.navigate(['/client-dashboard', id]);
  }


}