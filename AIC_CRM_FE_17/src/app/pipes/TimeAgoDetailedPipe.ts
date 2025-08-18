import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeAgoDetailed'
})
export class TimeAgoDetailedPipe implements PipeTransform {
  transform(value: Date | string | undefined): string {
    if (!value) return '';

    const now = new Date();
    const date = new Date(value);
    const diffMs = now.getTime() - date.getTime();

    const seconds = Math.floor(diffMs / 1000);
    const mins = Math.floor(seconds / 60);
    const hours = Math.floor(mins / 60);

    const remainingMins = mins % 60;
    const remainingSecs = seconds % 60;

    return `${hours} hours, ${remainingMins} mins, ${remainingSecs} secs ago`;
  }
}
