import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { NgFor } from '@angular/common';
import { TableModule } from 'primeng/table';
import { FileUploadModule } from 'primeng/fileupload';
import { ButtonModule } from 'primeng/button';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-signal-r-file',
  standalone: true,
  imports: [NgFor,ToastModule,ConfirmDialogModule,TableModule,FileUploadModule,ButtonModule],
  templateUrl: './signal-r-file.component.html',
  styleUrl: './signal-r-file.component.scss'
})
export class SignalRFileComponent {
  selectedFile: File | null = null;
  processedRows: string[] = [];
  columns: any[] = [];
  header: string = '';
  private hubConnection: HubConnection | undefined;

  constructor(private http: HttpClient) {}

  ngOnInit():void{
this.startSignalRConnection();
  }

  private startSignalRConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7052/fileProcessingHub')
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connection established'))
      .catch(err => console.error('Error while establishing SignalR connection:', err));

      this.hubConnection.on('ReceiveHeader', (header: string) => {
        console.log('Received header update:', header);
        this.columns = header.split(', ').map(column => ({ field: column, header: column }));
        // this.header = header;
      });

    this.hubConnection.on('ReceiveRowUpdate', (row: string) => {
      console.log('Received row update:', row);
      const rowData = row.split(', ');
      const rowObject: any = {};
      this.columns.forEach((col, index) => {
        rowObject[col.field] = rowData[index];
      });
      this.processedRows.push(rowObject);
    });

   
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    
  }
  onGetData(){
    this.getProcessedRows();
  }

  onUpload(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);

      this.http.post('https://localhost:7052/api/FileUpload', formData).subscribe(
        () => console.log('File uploaded successfully'),
        err => console.error('File upload failed', err)
      );
    }
  }

  getProcessedRows(): void {
    this.http.get<string[]>('https://localhost:7052/api/FileUpload/processedRows').subscribe(
      data => {
        console.log('Processed rows:', data);
        this.processedRows = data;
      },
      err => console.error('Failed to fetch processed rows', err)
    );
  }
  
}
