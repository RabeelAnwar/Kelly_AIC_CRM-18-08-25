import { Injectable } from "@angular/core";
import { CustomToastService } from "./custom-toast.service";

@Injectable({
    providedIn: 'root', // You can also use 'root' to make it globally available
})
export class FileDownload {



    constructor(
        private toastr: CustomToastService
    ) { }

    downloadFileByUrl(fileUrl: string) {
        fetch(fileUrl, {
            method: 'GET',
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.blob();
            })
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                const fileName = fileUrl.split('/').pop() || 'download.pdf';
                a.download = fileName;

                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);

                window.URL.revokeObjectURL(url);
            })
            .catch(err => {
                console.error('Download error:', err);
                this.toastr.error('Failed to download file');
            });
    }

}