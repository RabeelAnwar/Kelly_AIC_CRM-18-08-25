import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { RequisitionModel } from '../../../models/requisition/requisition-model';

@Component({
  selector: 'app-left-requisition-search-nav',
  templateUrl: './left-requisition-search-nav.component.html',
  styleUrls: ['./left-requisition-search-nav.component.css'],
})
export class LeftRequisitionSearchNavComponent implements OnInit {
  requisitionsList: RequisitionModel[] = [];
  filteredRequisitions: RequisitionModel[] = [];
  pagedRequisitions: RequisitionModel[] = [];
  searchText: string = '';

  currentPage: number = 1;
  pageSize: number = 12;
  totalPages: number = 1;

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const savedPage = localStorage.getItem('requisitionListPage');
    if (savedPage && !isNaN(+savedPage)) {
      this.currentPage = +savedPage; // ✅ use the saved page number
    } else {
      this.currentPage = 1;
    }
    this.getRequisitions();
  }

  getRequisitions(): void {
    this.apiService.getData('Client/ClientOpenRequisitionsListGet').subscribe({
      next: (response) => {
        this.requisitionsList = response?.data || [];
        this.requisitionsList.forEach((item) => {
          item.jobTitle = item.clientName + ' - ' + item.jobTitle;
        });
        this.filteredRequisitions = [...this.requisitionsList];
        this.sortAndPaginate();
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisitions');
      },
    });
  }

  onSearch(): void {
    const query = this.searchText.toLowerCase().trim();

    if (!query) {
      // this.filteredRequisitions = [...this.requisitionsList];
      this.getRequisitions();
    } else {
      this.filteredRequisitions = this.requisitionsList.filter(
        (item) =>
          item.jobTitle?.toLowerCase().includes(query) ||
          '' ||
          item.clientName?.toLowerCase().includes(query) ||
          '' ||
          item.clientReqNumber?.toLowerCase().includes(query) ||
          ''
      );
    }

    this.sortAndPaginate();
  }

  sortAndPaginate(): void {
    // Sort alphabetically by jobTitle
    this.filteredRequisitions.sort((a, b) => {
      const titleA = a.jobTitle?.toLowerCase() || '';
      const titleB = b.jobTitle?.toLowerCase() || '';
      return titleA.localeCompare(titleB);
    });

    this.currentPage = 1;
    this.totalPages =
      Math.ceil(this.filteredRequisitions.length / this.pageSize) || 1;
    this.updatePagedRequisitions();
  }

  changePage(delta: number): void {
    this.currentPage = Math.max(
      1,
      Math.min(this.currentPage + delta, this.totalPages)
    );
    this.updatePagedRequisitions();
  }

  updatePagedRequisitions(): void {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedRequisitions = this.filteredRequisitions.slice(start, end);
  }

  goToDashboard(id: number): void {
    localStorage.setItem('requisitionListPage', this.currentPage.toString());
    this.router.navigate(['/RequisitionDashboard', id]);
  }
  ngOnDestroy(): void {
    localStorage.removeItem('requisitionListPage');
  }
}
