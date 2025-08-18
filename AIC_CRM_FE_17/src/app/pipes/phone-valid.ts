import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneValid'
})
export class PhoneValidPipe implements PipeTransform {
  transform(value: string | undefined): boolean {
    if (!value) return true;
    const digitsOnly = value.replace(/\D/g, '');
    return digitsOnly.length >= 10 && digitsOnly.length <= 15;
  }
}
