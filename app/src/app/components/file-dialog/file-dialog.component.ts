import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { AtanetHttpService } from '../../services/atanet-http.service';

@Component({
  selector: 'app-file-dialog',
  templateUrl: './file-dialog.component.html',
  styleUrls: ['./file-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FileDialogComponent {
  private _file: File;
  private _fileId: number;

  constructor(
    public dialogRef: MatDialogRef<FileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private httpService: AtanetHttpService) {
  }

  public get file(): File {
    return this._file;
  }

  public close(): void {
    this.dialogRef.close(-1);
  }
}
