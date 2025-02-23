import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../features/auth/services/auth.service';

@Injectable()
export class AuthGuard {

  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(): boolean {
    console.log(this.authService.isAuthenticated());
    const isAuthenticated = this.authService.isAuthenticated();
    if (!isAuthenticated) {
      this.router.navigate(['/auth/login']);
      return false;  // Prevent navigation
    }
    return true;
  }
}
