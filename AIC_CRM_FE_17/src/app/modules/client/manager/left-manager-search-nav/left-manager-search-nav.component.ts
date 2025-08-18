import { Component, Input } from '@angular/core';
import { ApiService } from '../../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';
import { DropdownItem } from '../../../../models/common/common';

@Component({
  selector: 'app-left-manager-search-nav',
  templateUrl: './left-manager-search-nav.component.html',
  styleUrl: './left-manager-search-nav.component.css'
})
export class LeftManagerSearchNavComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  @Input() fildteredManagersIds: number[] = [];

  managersList: DropdownItem[] = [];
  managers: DropdownItem[] = [];
  allManagers: DropdownItem[] = [];

  ngOnInit(): void {
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
      this.allManagers = dropdownItems.filter(c => this.fildteredManagersIds.includes(c.id));
      this.managers = this.allManagers;
    }
    else {
      this.allManagers = dropdownItems;
      this.managers = dropdownItems;
    }
  }


  goToDashboard(id: number): void {

    const manager = this.managersList.find(c => c.id === id);
    if (manager) {
      this.router.navigate(['/ManagerDashboard', id], {
        state: { fildteredManagersIds: this.fildteredManagersIds }
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
          this.allManagers = dropdownItems.filter(c => this.fildteredManagersIds.includes(c.id));
          this.managers = this.allManagers;
        }
        else {
          this.allManagers = dropdownItems;
          this.managers = dropdownItems;
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
      }
    });
  }

}
