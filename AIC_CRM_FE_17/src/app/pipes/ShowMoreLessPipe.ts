import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'showMoreLess'
})
export class ShowMoreLessPipe implements PipeTransform {

  transform(value: string | undefined, limit: number = 100, showMore: boolean = false): string {
    if (!value) return '';
    if (value.length <= limit || showMore) {
      return value;
    }
    return value.slice(0, limit) + '...';
  }
}
