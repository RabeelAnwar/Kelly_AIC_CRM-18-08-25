import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'replaceNbsp'
})
export class ReplaceNbspPipe implements PipeTransform {
  transform(value: string | undefined): string {
    return value ? value.replace(/&nbsp;/g, ' ') : '';
  }
}
