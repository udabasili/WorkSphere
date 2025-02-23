import {Injectable} from '@angular/core';
import {ErrorHandlerService} from '../../../core/services/error-handler.service';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {environment} from '../../../../environments/environment';
import {AuthResponse} from '../models/auth-response';
import {User} from '../models/user';
import {BehaviorSubject, Observable, Subscription} from 'rxjs';
import {AuthStatus} from '../models/auth-status';

const API_URL = environment.apiUrl;

export interface LoginRequest {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private token: string = '';
  private authStatusListener = new BehaviorSubject<AuthStatus>({user: null, isAuthenticated: false});

  constructor(
    private http: HttpClient,
    private errorService: ErrorHandlerService,
    private router: Router
  ) {
  }

  login(credentials: LoginRequest): Subscription {
    return this.http.post<AuthResponse>(`${API_URL}/api/auth/login`, credentials).subscribe({
      next: (response) => {
        this.handleAuthResponse(response);
        return response;
      },
      error: (err) => {
        this.errorService.apiErrorHandler(err);
      }
    });
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getAuthStatusListener(): Observable<AuthStatus> {
    return this.authStatusListener.asObservable();
  }

  isAuthenticated(): boolean {
    return !!this.getToken();  // Check if token exists and is valid
  }

  logout(): void {
    this.token = '';
    localStorage.removeItem('token');
    this.authStatusListener.next({user: null, isAuthenticated: false});
    this.router.navigate(['/auth/login']);
  }

  verifyUser(): void {
    this.http.get<AuthResponse>(`${API_URL}/api/auth/verify-token`).subscribe({
      next: (response) => {
        if (response.token) {
          this.handleAuthResponse(response);
        }
      },
      error: (err) => {
        this.logout();
        this.errorService.apiErrorHandler(err);
      }
    });
  }

  private handleAuthResponse(response: AuthResponse): void {
    this.token = response.token;
    const {expiresIn, user} = response;
    const expirationDate = new Date(new Date().getTime() + expiresIn * 1000);

    this.authStatusListener.next({user, isAuthenticated: true});
    this.saveAuthData(response.token, expirationDate, user);
    this.setAuthTimer(expiresIn);

    this.router.navigate(['/']);
  }

  private saveAuthData(token: string, expirationDate: Date, user: User): void {
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('tokenExpiration', expirationDate.toISOString());
  }

  private setAuthTimer(duration: number): void {
    setTimeout(() => {
      this.logout();
    }, duration * 1000);
  }
}
