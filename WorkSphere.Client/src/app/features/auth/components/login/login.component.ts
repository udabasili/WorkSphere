import {Component, OnDestroy, OnInit} from '@angular/core';
import {NgForm} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Subscription} from 'rxjs';

const adminLogin = {
  email: 'admin@test.com',
  password: 'Password123!'
}

@Component({
  selector: 'app-login',
  standalone: false,

  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {
  loginData = {...adminLogin}
  buttonLoading: boolean = false;
  private authSub?: Subscription;

  constructor(
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.authSub = this.authService.getAuthStatusListener().subscribe({
      next: (authStatus) => {
        console.log(authStatus);
        this.buttonLoading = false;
      },
      error: (err) => {
        console.log(err);
        this.buttonLoading = false;
      }
    })
  }

  ngOnDestroy(): void {
    this.authSub?.unsubscribe();
  }

  onSubmit(loginForm: NgForm) {
    this.buttonLoading = true;
    if (loginForm.invalid) {
      return;
    }
    this.authService.login(this.loginData).add(() => {
      this.buttonLoading = false;
    });

  }

  setAdminLogin(loginForm: NgForm) {
    loginForm.resetForm(adminLogin);
  }

}
