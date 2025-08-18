import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { ForgotPasswordComponent } from './modules/auth/forgot-password/forgot-password.component';
import { CompanyRegisterComponent } from './modules/auth/company-register/company-register.component';
import { DashboardComponent } from './modules/admin/dashboard/dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'company-register', component: CompanyRegisterComponent },
  { path: 'company-profile', component: CompanyRegisterComponent },
  { path: 'home', component: DashboardComponent },
  {
    path: '..',
    loadChildren: () =>
      import('./modules/admin/admin.module').then(
        (m) => m.AdminModule
      ),
  }

  // {    path: 'user-e',    component: UserComponent,    canActivate: [authGuard],   },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
