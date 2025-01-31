export interface ApiErrorResponse {
  type: string;
  title: string;
  status: number;
  errors: { [key: string]: string[] };
  traceId: string;
}
