import {DatePipe} from '@angular/common';

/**
 * Formats a given date using Angular's DatePipe.
 * @param date The input date value
 * @param datePipe Angular's DatePipe instance
 * @returns Formatted date as a string
 */
export function inputDateFormat(date: string | Date | null, datePipe: DatePipe): string {
  return date ? datePipe.transform(date, 'yyyy-MM-dd') ?? '' : '';
}
