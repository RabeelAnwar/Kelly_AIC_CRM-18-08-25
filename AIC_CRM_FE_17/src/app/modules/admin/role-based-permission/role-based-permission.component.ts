import { Component } from '@angular/core';

@Component({
  selector: 'app-role-based-permission',
  templateUrl: './role-based-permission.component.html',
  styleUrl: './role-based-permission.component.css'
})
export class RoleBasedPermissionComponent {
  selectedCountry: any;
  selectedState: any;
  selectedCity: any;

  cars = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
    { id: 4, name: 'Audi' },
  ];
}
