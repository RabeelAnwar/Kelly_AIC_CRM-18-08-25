// import { Component, Input } from '@angular/core';
// import { ClientManagerModel } from '../../../../models/client/client-manager-model';
// import { Router } from '@angular/router';

// @Component({
//   selector: 'app-right-manager-search-nav',
//   templateUrl: './right-manager-search-nav.component.html',
//   styleUrl: './right-manager-search-nav.component.css'
// })
// export class RightManagerSearchNavComponent {

//   @Input() clientManagers: ClientManagerModel[] = [];
//   @Input() fromClientDashboard: boolean = false;

//   filteredManagers: ClientManagerModel[] = [];
//   searchText: string = '';

//   constructor(
//     private router: Router,
//   ) { }

//   ngOnChanges(): void {

//     this.filteredManagers = [...this.clientManagers];
//     this.sortFilteredManagers();
//   }

//   onSearch(): void {
//     const search = this.searchText.trim().toLowerCase();

//     if (!search) {
//       // If no search text, return all managers sorted
//       this.filteredManagers = [...this.clientManagers];
//       this.sortFilteredManagers();
//       return;
//     }

//     const startsWith = this.clientManagers
//       .filter(manager => {
//         const fullName = `${manager.firstName ?? ''} ${manager.lastName ?? ''}`.toLowerCase();
//         return fullName.startsWith(search);
//       })
//       .sort((a, b) => {
//         const nameA = `${a.firstName ?? ''} ${a.lastName ?? ''}`.toLowerCase();
//         const nameB = `${b.firstName ?? ''} ${b.lastName ?? ''}`.toLowerCase();
//         return nameA.localeCompare(nameB);
//       });

//     const contains = this.clientManagers
//       .filter(manager => {
//         const fullName = `${manager.firstName ?? ''} ${manager.lastName ?? ''}`.toLowerCase();
//         return !fullName.startsWith(search) && fullName.includes(search);
//       })
//       .sort((a, b) => {
//         const nameA = `${a.firstName ?? ''} ${a.lastName ?? ''}`.toLowerCase();
//         const nameB = `${b.firstName ?? ''} ${b.lastName ?? ''}`.toLowerCase();
//         return nameA.localeCompare(nameB);
//       });

//     this.filteredManagers = [...startsWith, ...contains];
//   }

//   private sortFilteredManagers(): void {
//     this.filteredManagers.sort((a, b) => {
//       const nameA = `${a.firstName ?? ''} ${a.lastName ?? ''}`.toLowerCase();
//       const nameB = `${b.firstName ?? ''} ${b.lastName ?? ''}`.toLowerCase();
//       return nameA.localeCompare(nameB);
//     });
//   }



//   goToDashboard(id: number): void {
//     const manager = this.clientManagers.find(c => c.id === id);
//     if (manager) {
//       this.router.navigate(['/ManagerDashboard', id], {
//         state: { fromClientDashboard: this.fromClientDashboard }
//       });
//     }
//   }

// }



import { Component, Input, OnChanges } from '@angular/core';
import { ClientManagerModel } from '../../../../models/client/client-manager-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-right-manager-search-nav',
  templateUrl: './right-manager-search-nav.component.html',
  styleUrls: ['./right-manager-search-nav.component.css']
})
export class RightManagerSearchNavComponent implements OnChanges {

  @Input() clientManagers: ClientManagerModel[] = [];
  @Input() fromClientDashboard: boolean = false;

  filteredManagers: ClientManagerModel[] = [];
  searchText: string = '';

  constructor(private router: Router) {}

  ngOnChanges(): void {
    this.filteredManagers = [...this.clientManagers];
    this.sortFilteredManagers(); // Sorting updated below
  }

  onSearch(): void {
    const search = this.searchText.trim().toLowerCase();

    if (!search) {
      this.filteredManagers = [...this.clientManagers];
      this.sortFilteredManagers(); // Sorting updated below
      return;
    }

    const startsWith = this.clientManagers
      .filter(manager => {
        const fullName = `${manager.firstName ?? ''} ${manager.lastName ?? ''}`.toLowerCase();
        return fullName.startsWith(search);
      })
      .sort(this.sortByLastThenFirst); // 🔁 Updated: Sort by last name, then first name

    const contains = this.clientManagers
      .filter(manager => {
        const fullName = `${manager.firstName ?? ''} ${manager.lastName ?? ''}`.toLowerCase();
        return !fullName.startsWith(search) && fullName.includes(search);
      })
      .sort(this.sortByLastThenFirst); // 🔁 Updated: Sort by last name, then first name

    this.filteredManagers = [...startsWith, ...contains];
  }

  private sortFilteredManagers(): void {
    this.filteredManagers.sort(this.sortByLastThenFirst); // 🔁 Updated: Sort by last name, then first name
  }

  // 🔧 New: Reusable method to sort by last name, then first name
  private sortByLastThenFirst(a: ClientManagerModel, b: ClientManagerModel): number {
    const lastNameA = (a.lastName ?? '').toLowerCase();
    const lastNameB = (b.lastName ?? '').toLowerCase();

    if (lastNameA !== lastNameB) {
      return lastNameA.localeCompare(lastNameB);
    }

    const firstNameA = (a.firstName ?? '').toLowerCase();
    const firstNameB = (b.firstName ?? '').toLowerCase();
    return firstNameA.localeCompare(firstNameB);
  }

  goToDashboard(id: number): void {
    const manager = this.clientManagers.find(c => c.id === id);
    if (manager) {
      this.router.navigate(['/ManagerDashboard', id], {
        state: { fromClientDashboard: this.fromClientDashboard }
      });
    }
  }
}
