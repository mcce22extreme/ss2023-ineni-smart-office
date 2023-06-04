import {Component, Inject} from '@angular/core';
import {MatDialog, MAT_DIALOG_DATA, MatDialogModule} from '@angular/material/dialog';
import {NgIf} from '@angular/common';

export interface DialogData {
    message: '';
  }

@Component({
    selector: 'delete-dialog',
    templateUrl: 'delete-dialog.component.html',
    styleUrls: ['delete-dialog.component.scss']
  })
  export class DeleteDialogComponent {
    constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData) {}
  }