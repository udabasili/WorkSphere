import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => this.handleError(err))
    );
  }

  protected handleError(err: HttpErrorResponse): Observable<never> {
    // Network-based errors
    if (err.error instanceof ProgressEvent) {
      console.error('Progress event error detected:', err);
      return throwError(() => new Error('Error connecting to REST Server'));
    }

    // Server-side errors
    return throwError(() => err);
  }
}
