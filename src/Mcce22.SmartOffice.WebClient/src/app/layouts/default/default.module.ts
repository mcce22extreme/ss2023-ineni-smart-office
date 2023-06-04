import { NgModule,  } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { MatSidenavModule } from '@angular/material/sidenav'
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { DefaultComponent } from './default.component';

import { DashboardComponent } from 'src/app/modules/dashboard/dashboard.component';
import { SharedModule } from 'src/app/shared/shared/shared.module';
import { UserListComponent } from 'src/app/modules/users/user-list.component';
import { UserService } from 'src/app/modules/users/user.service';
import { UserDetailComponent } from 'src/app/modules/users/user-detail.component';

@NgModule({
  providers:[
    UserService
  ],
  declarations: [
    DefaultComponent,
    DashboardComponent,
    UserListComponent,
    UserDetailComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    SharedModule,
    MatSidenavModule,
    MatDividerModule,
    MatGridListModule,
    MatCardModule,
    MatIconModule,
    MatListModule, 
    MatButtonModule,
    MatMenuModule,
    MatTableModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule
  ]
})
export class DefaultModule { }
