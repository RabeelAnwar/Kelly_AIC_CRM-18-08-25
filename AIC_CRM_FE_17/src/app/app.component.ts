import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  // imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AIC_CRM_FE';
  constructor(private router: Router) { }

  isLoginPage: boolean = true;

  componentsList = ['/login', '/forgot-password', '/company-register']

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoginPage = this.componentsList.includes(event.url) || event.url === '/';
      }
    });
  }
}
