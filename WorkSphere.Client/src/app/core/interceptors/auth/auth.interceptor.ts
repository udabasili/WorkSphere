import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {AuthService} from '../../../features/auth/services/auth.service';
import {Observable} from 'rxjs';
import {Injectable} from '@angular/core';

@Injectable()

export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authToken = this.authService.getToken();
    const authRequest = req.clone({
      headers: req.headers.set('Authorization', 'Bearer ' + authToken)
    });
    //send the request with the token
    return next.handle(authRequest);
  }
}
