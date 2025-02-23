import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from './features/auth/services/auth.service';
import {Router} from '@angular/router';
import {Subscription} from 'rxjs';
import {User} from './features/auth/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: false
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'WorkSphere.Client';
  userIsAuthenticated = false;
  user: User | null = null;
  authSub?: Subscription;
  isLoading = true;

  constructor(private authService: AuthService, public router: Router) {
  }

  ngOnInit(): void {
    const token = this.authService.getToken();
    if (token) {
      this.authService.verifyUser();
    }

    this.timerTest();

    this.authSub = this.authService.getAuthStatusListener().subscribe({
      next: (response) => {
        this.userIsAuthenticated = response.isAuthenticated;
        if (this.userIsAuthenticated) {
          this.user = response.user;
        }
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  ngOnDestroy(): void {
    if (this.authSub) {
      this.authSub.unsubscribe();
    }
  }

  private timerTest(): void {
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }
}
