import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { Router } from '@angular/router';
import { DropdownItem } from '../../../models/common/common';

@Component({
  selector: 'app-right-requisition-search-nav',
  templateUrl: './right-requisition-search-nav.component.html',
  styleUrls: ['./right-requisition-search-nav.component.css']
})
export class RightRequisitionSearchNavComponent implements OnChanges {

  @Input() clientRequisition: RequisitionModel[] = [];

  requisitionsList: DropdownItem[] = [];
  filteredRequisitions: DropdownItem[] = [];
  searchText: string = '';

  constructor(private router: Router) { }

  ngOnChanges(): void {
    this.filteredRequisitions = [];

    this.clientRequisition.forEach(i => {
      this.filteredRequisitions.push({
        id: i.id,
        name: `Req# ${i.id} ${i.jobTitle}`
      });
    });

    this.requisitionsList = [...this.filteredRequisitions];

    this.sortFilteredRequisitions();
  }

  onSearch(): void {
    const search = this.searchText.trim().toLowerCase();

    if (!search) {
      this.filteredRequisitions = [...this.requisitionsList];
      this.sortFilteredRequisitions();
      return;
    }

    const startsWith = this.requisitionsList
      .filter(item => item.name.toLowerCase().startsWith(search))
      .sort((a, b) => a.name.toLowerCase().localeCompare(b.name.toLowerCase()));

    const contains = this.requisitionsList
      .filter(item => !item.name.toLowerCase().startsWith(search) && item.name.toLowerCase().includes(search))
      .sort((a, b) => a.name.toLowerCase().localeCompare(b.name.toLowerCase()));

    this.filteredRequisitions = [...startsWith, ...contains];
  }

  private sortFilteredRequisitions(): void {
    this.filteredRequisitions.sort((a, b) => a.name.toLowerCase().localeCompare(b.name.toLowerCase()));
  }

  goToDashboard(id: number): void {
    const requisition = this.clientRequisition.find(c => c.id === id);
    debugger;
    if (requisition) {
      this.router.navigate(['/RequisitionDashboard', id], {
        state: { requisition }

      });
    }
  }

}
