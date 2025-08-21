import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../../services/api.service';
import { DropdownItem } from '../../../models/common/common';
import { ConsultantModel } from '../../../models/consultant/consultant-model';

@Component({
  selector: 'app-left-consultant-search-nav',
  templateUrl: './left-consultant-search-nav.component.html',
  styleUrl: './left-consultant-search-nav.component.css'
})
export class LeftConsultantSearchNavComponent {

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService,
    private router: Router,

  ) { }

  consultantsList: ConsultantModel[] = [];
  consultants: DropdownItem[] = [];
  allconsultants: DropdownItem[] = [];
  @Input() fildteredConsultantsIds: number[] = [];

  searchText: string = '';

  pageSize = 15;
  currentPage = 1;
  totalPages = 1;

  // ngOnInit(): void {
  //   this.getConsultants();
  // }
  ngOnInit(): void {
    const savedPage = localStorage.getItem('consultantListPage');
    if (savedPage && !isNaN(+savedPage)) {
      this.currentPage = +savedPage; // ✅ use the saved page number
    } else {
      this.currentPage = 1;
    }

    this.getConsultants();
  }
  // ngOnDestroy(): void {
  //   localStorage.removeItem('consultantListPage');
  // }

  ngOnChanges(): void {

    const dropdownItems: DropdownItem[] = [];
    this.consultantsList.forEach((c: any) => {
      dropdownItems.push({
        id: c.id,
        name: c.lastName + ', ' + c.firstName,
      });
    });

    if (this.fildteredConsultantsIds.length > 0) {
      this.allconsultants = dropdownItems.filter(c => this.fildteredConsultantsIds.includes(c.id));
      this.consultants = this.allconsultants;
    }
    else {
      this.allconsultants = dropdownItems;
      this.consultants = dropdownItems;
    }
  }



  onSearch(): void {
    const text = this.searchText.toLowerCase().trim();

    if (!text) {
      this.consultants = [...this.allconsultants].sort((a, b) =>
        a.name.localeCompare(b.name)
      );
      return;
    }

    const startsWith = this.allconsultants
      .filter(c => c.name.toLowerCase().startsWith(text))
      .sort((a, b) => a.name.localeCompare(b.name));

    const contains = this.allconsultants
      .filter(c => !c.name.toLowerCase().startsWith(text) && c.name.toLowerCase().includes(text))
      .sort((a, b) => a.name.localeCompare(b.name));

    this.consultants = [...startsWith, ...contains];
  }



  onAlphabetFilter(letter: string): void {

    const text = this.searchText.toLowerCase().trim();

    let filtered = this.allconsultants;

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

    this.consultants = filtered.sort((a, b) => a.name.localeCompare(b.name));
  }


  // goToDashboard(id: number): void {

  //   const consultant = this.consultantsList.find(c => c.id === id);
  //   if (consultant) {
  //     this.router.navigate(['/consultant-dashboard', id], {
  //       state: { fildteredConsultantsIds: this.fildteredConsultantsIds }

  //     });
  //   } else {
  //     this.toastr.error('Not found');
  //   }

  // }
  goToDashboard(id: number): void {
    const consultant = this.consultantsList.find(c => c.id === id);
    if (consultant) {
      localStorage.setItem('consultantListPage', this.currentPage.toString());
      this.router.navigate(['/consultant-dashboard', id], {
        state: {
          fildteredConsultantsIds: this.fildteredConsultantsIds,
          // previousPage: this.currentPage // pass the page number
        }
      });
    } else {
      this.toastr.error('Not found');
    }
  }


  getConsultants() {
    this.apiService.getData('Consultant/ConsultantsListGet').subscribe({
      next: (response) => {
        this.consultantsList = response?.data || [];

        this.consultantsList = (response?.data || []).sort((a: any, b: any) => {
          const lastNameA = a.lastName?.toLowerCase() || "";
          const lastNameB = b.lastName?.toLowerCase() || "";
          return lastNameA.localeCompare(lastNameB);
        });


        const dropdownItems: DropdownItem[] = [];

        response?.data.forEach((c: any) => {
          dropdownItems.push({
            id: c.id,
            name: c.lastName + ', ' + c.firstName,
          });
        });

        if (this.fildteredConsultantsIds.length > 0) {
          this.allconsultants = dropdownItems.filter(c => this.fildteredConsultantsIds.includes(c.id));
          this.consultants = this.allconsultants;
        }
        else {
          // this.allconsultants = dropdownItems;
          // this.consultants = dropdownItems;
          this.allconsultants = dropdownItems;
          // this.totalPages = Math.ceil(this.allconsultants.length / this.pageSize);
          // this.setPage(1);
          this.totalPages = Math.ceil(this.allconsultants.length / this.pageSize);
          this.setPage(this.currentPage);
        }

      },
      error: (err) => {
        this.toastr.error(err || 'Error loading consultants list');
      }
    });
  }
  setPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;

    this.currentPage = page;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.consultants = this.allconsultants.slice(startIndex, endIndex);

  }
}
