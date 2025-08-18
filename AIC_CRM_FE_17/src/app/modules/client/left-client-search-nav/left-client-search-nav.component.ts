import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { ClientModel } from '../../../models/client/client-model';
import { DropdownItem } from '../../../models/common/common';

@Component({
  selector: 'app-left-client-search-nav',
  templateUrl: './left-client-search-nav.component.html',
  styleUrl: './left-client-search-nav.component.css'
})
export class LeftClientSearchNavComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  clientsList: ClientModel[] = [];
  client: DropdownItem[] = [];
  allClients: DropdownItem[] = [];  // Store the original list
  searchText: string = '';

  pageSize = 15;
  currentPage = 1;
  totalPages = 1;

  ngOnInit(): void {
    this.getClients();
  }


  // onSearch(): void {
  //   const text = this.searchText.toLowerCase().trim();

  //   if (!text) {
  //     this.client = [...this.allClients].sort((a, b) =>
  //       a.name.localeCompare(b.name)
  //     );
  //     return;
  //   }

  //   this.client = this.allClients
  //     .filter(c => c.name.toLowerCase().includes(text))
  //     .sort((a, b) => a.name.localeCompare(b.name));
  // }

  onSearch(): void {
    const text = this.searchText.toLowerCase().trim();

    if (!text) {
      this.client = [...this.allClients].sort((a, b) =>
        a.name.localeCompare(b.name)
      );
      return;
    }

    const startsWith = this.allClients
      .filter(c => c.name.toLowerCase().startsWith(text))
      .sort((a, b) => a.name.localeCompare(b.name));

    const contains = this.allClients
      .filter(c => !c.name.toLowerCase().startsWith(text) && c.name.toLowerCase().includes(text))
      .sort((a, b) => a.name.localeCompare(b.name));

    this.client = [...startsWith, ...contains];
  }


  onAlphabetFilter(letter: string): void {
    const text = this.searchText.toLowerCase().trim();

    let filtered = this.allClients;

    if (text) {
      filtered = filtered.filter(c =>
        c.name.toLowerCase().includes(text)
      );
    }

    if (letter === 'All') {
      // No further filtering
    } else if (letter === '#') {
      filtered = filtered.filter(c => !/^[a-zA-Z]/.test(c.name));
    } else {
      const lowerLetter = letter.toLowerCase();
      filtered = filtered.filter(c => c.name.toLowerCase().startsWith(lowerLetter));
    }

    this.client = filtered.sort((a, b) => a.name.localeCompare(b.name));
  }


  goToDashboard(id: number): void {

    const client = this.clientsList.find(c => c.id === id);
    if (client) {
      this.router.navigate(['/client-dashboard', id], {
        state: { client }
      });
      // this.clientService.setClient(client);
      // this.router.navigate(['/client-dashboard', id]);
    } else {
      this.toastr.error('Client not found');
    }

  }


  getClients(): void {
    this.apiService.getData('Client/ClientsListGet').subscribe({
      next: (response) => {
        this.clientsList = response?.data || [];
        const dropdownItems: DropdownItem[] = [];

        response?.data.forEach((client: any) => {
          dropdownItems.push({
            id: client.id,
            name: client.clientName
          });
        });

        // this.allClients = dropdownItems;
        // this.client = dropdownItems;
          this.allClients = dropdownItems;
        this.totalPages = Math.ceil(this.allClients.length / this.pageSize);
        this.setPage(1);
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading clients list');
      }
    });
  }

  setPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;

    this.currentPage = page;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.client = this.allClients.slice(startIndex, endIndex);

  }
}