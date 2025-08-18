import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class ReportsExport {

  exportConsultantsListToWord(consultantsList: any[], fileName: string = 'Report') {
    const header = `
      <html xmlns:o='urn:schemas-microsoft-com:office:office' 
            xmlns:w='urn:schemas-microsoft-com:office:word' 
            xmlns='http://www.w3.org/TR/REC-html40'>
        <head>
          <meta charset='utf-8'>
          <title>Export HTML to Word</title>
          <style>
            @page {
              margin: 0.5in 0.3in 0.5in 0.3in;
            }
            body {
              margin: 0.5in 0.3in 0.5in 0.3in;
              font-family: Arial, sans-serif;
              font-size: 11pt;
            }
            table {
              border-collapse: collapse;
              width: 100%;
              border: 1px solid black;
              table-layout: fixed;
              word-wrap: break-word;
              word-break: break-word;
            }
            th, td {
              border: 1px solid black;
              padding: 6px 8px;
              text-align: left;
              max-width: 150px;
              overflow-wrap: break-word;
            }
          </style>
        </head>
        <body>
          <table>
            <thead>
              <tr>
                <th>Sr</th>
                <th>Name</th>
                <th>Address</th>
                <th>Current Position</th>
                <th>State</th>
                <th>City</th>
                <th>Phone</th>
                <th>Email</th>
                <th>Call Records</th>
              </tr>
            </thead>
            <tbody>
    `;

    let body = '';
    consultantsList.forEach((consultant, index) => {
      body += `
        <tr>
          <td>${index + 1}</td>
          <td>${consultant.lastName ?? ''} ${consultant.firstName ?? ''}</td>
          <td>${consultant.completeAddress ?? ''}</td>
          <td>${consultant.currentPosition ?? ''}</td>
          <td>${consultant.state ?? ''}</td>
          <td>${consultant.city ?? ''}</td>
          <td>${consultant.cellPhone ?? ''}</td>
          <td>${consultant.workEmail ?? ''}</td>
          <td>${consultant.callRecords ?? ''}</td>
        </tr>
      `;
    });

    const footer = `
            </tbody>
          </table>
        </body>
      </html>
    `;

    const sourceHTML = header + body + footer;
    const source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
    const fileDownload = document.createElement('a');
    document.body.appendChild(fileDownload);
    fileDownload.href = source;
    fileDownload.download = `${fileName}.doc`;
    fileDownload.click();
    document.body.removeChild(fileDownload);
  }
}
