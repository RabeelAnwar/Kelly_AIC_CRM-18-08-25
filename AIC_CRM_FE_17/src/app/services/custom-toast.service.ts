import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
    providedIn: 'root',
})
export class CustomToastService {
    constructor(private toastr: MessageService) { }

    success(message: string) {
        this.toastr.add({
            severity: 'success',
            detail: message,
        });
    }

    error(message: string) {
        this.toastr.add({
            severity: 'danger',
            detail: message,
        });
    }

    info(message: string) {
        this.toastr.add({
            severity: 'info',
            detail: message,
        });
    }

    warning(message: string) {
        this.toastr.add({
            severity: 'warning',
            detail: message,
        });
    }

    contrast(message: string) {
        this.toastr.add({
            severity: 'contrast',
            detail: message,
        });
    }
}