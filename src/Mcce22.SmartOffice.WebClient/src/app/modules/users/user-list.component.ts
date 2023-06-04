import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from './user.service';
import { MatTable } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { User } from './user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  displayedColumns: string[] = ['id','userName','firstName','lastName','email','actions'];
  users:User[] = [];

  @ViewChild(MatTable) table: MatTable<User> | undefined;

  constructor(private userService:UserService, private dialog: MatDialog, private router: Router){
  }

  ngOnInit() {
    this.loadUsers();
  }

  async loadUsers(){
    this.users = await lastValueFrom(this.userService.getUsers());
    this.table?.renderRows();
  }

  createUser(){
    this.router.navigate(['/users/create/']);
  }

  editUser(user:User){
    console.log(user);      
    this.router.navigate([`/users/edit/${user.id}`]);
  }

  deleteUser(user:User){
    console.log(user);

    var msg = `Delete user '${user.userName}'?`;
    console.log(msg);

    const dialogRef = this.dialog.open(DeleteDialogComponent, {
        panelClass: 'dialog-panel',
        data: {
          message: msg,
        },
      });

      dialogRef.afterClosed().subscribe(result => {
        if(result === true){
            this.userService.deleteUser(user.id);    
            this.loadUsers();
        }
      });
  }
}
