import { Directive, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Directive({
    selector: '[phoneValid]'
})
export class PhoneValidDirective {
    @Input('phoneValid') modelValue?: string;
    @Output() phoneValidFlagChange = new EventEmitter<boolean>();
    @Input() phoneValidFlag: boolean = false;

    @HostListener('input', ['$event.target.value'])
    onInput(value: string) {
        const digitsOnly = value.replace(/\D/g, '');

        if (digitsOnly == "") {
            this.phoneValidFlagChange.emit(true);
        }
        else {
            const isValid = digitsOnly.length >= 10 && digitsOnly.length <= 15;
            this.phoneValidFlagChange.emit(isValid);
        }
    }
}
