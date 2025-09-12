import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { ApiService } from '../../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';
import { DropdownItem } from '../../../../models/common/common';

@Component({
  selector: 'app-left-manager-search-nav',
  templateUrl: './left-manager-search-nav.component.html',
  styleUrl: './left-manager-search-nav.component.css',
})
export class LeftManagerSearchNavComponent
  implements OnInit, OnChanges, OnDestroy
{
  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  @Input() fildteredManagersIds: number[] = [];

  managersList: DropdownItem[] = [];
  managers: DropdownItem[] = [];
  allManagers: DropdownItem[] = [];
  pageSize = 15;
  currentPage = 1;
  totalPages = 1;
  ngOnInit(): void {
    const savedPage = localStorage.getItem('managerListPage');

    if (savedPage && !isNaN(+savedPage)) {
      this.currentPage = +savedPage; // ✅ use the saved page number
    } else {
      this.currentPage = 1;
    }
    this.getManagers();
  }

  ngOnChanges(): void {
    const dropdownItems: DropdownItem[] = [];
    this.managersList.forEach((c: any) => {
      dropdownItems.push({
        id: c.id,
        name: c.lastName + ', ' + c.firstName + '(' + c.clientName + ')',
      });
    });

    if (this.fildteredManagersIds.length > 0) {
      this.allManagers = dropdownItems.filter((c) =>
        this.fildteredManagersIds.includes(c.id)
      );
      this.managers = this.allManagers;
    } else {
      this.allManagers = dropdownItems;
      this.managers = dropdownItems;
    }
  }

  goToDashboard(id: number): void {
    const manager = this.managersList.find((c) => c.id === id);
    if (manager) {
      localStorage.setItem('managerListPage', this.currentPage.toString());
      this.router.navigate(['/ManagerDashboard', id], {
        state: { fildteredManagersIds: this.fildteredManagersIds },
      });
    }
  }

  getManagers() {
    this.apiService.getData('Client/ClientManagersListGet').subscribe({
      next: (response) => {
        this.managersList = response?.data || [];

        const dropdownItems: DropdownItem[] = [];

        this.managersList.forEach((c: any) => {
          dropdownItems.push({
            id: c.id,
            name: c.lastName + ', ' + c.firstName + ' (' + c.clientName + ')',
          });
        });

        if (this.fildteredManagersIds.length > 0) {
          this.allManagers = dropdownItems.filter((c) =>
            this.fildteredManagersIds.includes(c.id)
          );
          this.managers = this.allManagers;
        } else {
          this.allManagers = dropdownItems;
          //this.managers = dropdownItems;
          this.totalPages = Math.ceil(this.allManagers.length / this.pageSize);
          this.setPage(this.currentPage);
        }

        // this.managers = dropdownItems

        // const allManagers = response?.data || [];

        // const uniqueClientsMap = new Map();

        // for (const manager of allManagers) {
        //   if (!uniqueClientsMap.has(manager.clientId)) {
        //     uniqueClientsMap.set(manager.clientId, manager);
        //   }
        // }
        // this.managersList = Array.from(uniqueClientsMap.values());

        console.log(this.managersList);
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading managers list');
      },
    });
  }
  setPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;

    this.currentPage = page;
    localStorage.setItem('managerListPage', this.currentPage.toString()); // Save page

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.managers = this.allManagers.slice(startIndex, endIndex);
  }

  ngOnDestroy(): void {
    localStorage.removeItem('managerListPage');
  }
}
