import {Injectable} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {ToastService} from '../../services/toast.service';

interface ApiError {
  id: number;
  description: string;
  errorType: number;
}

interface ApiErrorResponse {
  type?: string;
  title?: string;
  status?: number;
  errors?: ApiError[];
  traceId?: string;
}


@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor(private toastService: ToastService) {
  }

  /**
   * Handle API errors from the ASP.NET Core API
   * @param err
   */
  apiErrorHandler(err: HttpErrorResponse): void {
    if (err.error instanceof ProgressEvent || err.status === 0) {
      // Network or connection issues
      this.toastService.showError('Network error: Unable to connect to the server.');
      return;
    }

    if (err.status >= 400 && err.status < 500) {
      // Handle client-side errors (400-499)
      this.handleClientErrors(err);
    } else if (err.status >= 500) {
      // Handle server-side errors (500+)
      this.toastService.showError('Server error: Please try again later or contact support.');
    } else {
      // Generic error message
      this.toastService.showError('An unexpected error occurred.');
    }

    console.error('API Error:', err);
  }

  /**
   * Handle client-side errors (400-499)
   * @param err
   * @private
   */
  private handleClientErrors(err: HttpErrorResponse): void {
    const error = err.error as ApiErrorResponse;

    if (err.status === 401) {
      this.toastService.showError('Unauthorized: Please log in again.');
      return;
    }

    if (err.status === 403) {
      this.toastService.showError('Forbidden: You do not have permission to perform this action.');
      return;
    }

    if (err.status === 404) {
      this.toastService.showError('Resource not found: The requested API endpoint does not exist.');
      return;
    }

    if (err.status === 405) {
      this.toastService.showError('Method Not Allowed: The requested method is not supported.');
      return;
    }

    if (err.status === 400 && error.errors) {
      // Handle validation errors from the API response
      error.errors.forEach((apiError: ApiError) => {
        this.toastService.showError(apiError.description);
      });
      return;
    }

    if (error.title) {
      this.toastService.showError(error.title);
    } else {
      this.toastService.showError(`Client error (${err.status}): ${err.statusText}`);
    }
  }
}
