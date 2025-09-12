import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from '../../../models/auth/login';
import { ApiService } from '../../../services/api.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { AdminModule } from '../../admin/admin.module';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(
    private apiService: ApiService,
    private auth: AuthService,
    private toastr: ToastrService,
    private router: Router,

  ) {
  }


  loginModel: LoginModel = new LoginModel();

  login() {
    if (this.loginModel.companyId === '' || this.loginModel.companyId === undefined || this.loginModel.companyId === null) {
      this.toastr.error('Please Enter Company');
      return;
    }
    if (this.loginModel.userName === '' || this.loginModel.password === '') {
      this.toastr.error('Please Enter UserName and Password');
      return;
    }


    this.apiService.saveData('Auth/Login', this.loginModel).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.auth.storeUserData(res.data);
          console.log(res.data.token);
          this.router.navigate(['/home']);
          this.toastr.success('Login Successfully');
        }
        else {
          this.toastr.error(res.message);
        }
      },
      error: (err) => this.toastr.error(err)
    });


    // if (
    //   this.loginModel.email.toLowerCase() === 'admin' &&
    //   this.loginModel.password === '123' &&
    //   this.loginModel.companyId.toLowerCase() === 'corporate'
    // ) {

    //   this.apiService
    //     .saveData('Auth/Login', this.loginModel)
    //     .subscribe((result: any) => {
    //       this.router.navigate(['/home']);
    //       this.toastr.success('Login Successfully');

    //       // if (result.succeeded == true ) {
    //       //   this.toastr.success('Save Successfully');
    //       // } else {
    //       //   this.toastr.error('Please Save Again');
    //       // }
    //     });
    // }

    // else {
    //   this.toastr.error('Invalid Credentials');
    //   return;
    // }
  }

}