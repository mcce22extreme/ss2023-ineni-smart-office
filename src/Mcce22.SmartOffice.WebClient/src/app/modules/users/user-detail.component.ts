import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from './user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from './user.model';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {

    userForm = new FormGroup({
        userName: new FormControl('', Validators.required),
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required),
    });

  id: string= '';
  fullName: string= '';
  newUser = false;

  constructor(private userService:UserService, private activatedRoute: ActivatedRoute, private router: Router){
  }

  ngOnInit() {    

    this.activatedRoute.data.subscribe(
        ({user}) => {
            if(user)
            {
                this.id = user.id;
                this.fullName = `${user.firstName} ${user.lastName} (${user.userName})`;
                this.userForm.controls.userName.patchValue(user.userName);
                this.userForm.controls.firstName.patchValue(user.firstName);
                this.userForm.controls.lastName.patchValue(user.lastName);
                this.userForm.controls.email.patchValue(user.email);
            }else{
                this.newUser = true;
            }
        });
  }

  save(){
    console.log(this.userForm.value.userName);

    this.userService.saveUser({
        id: this.id,
        userName: String(this.userForm.controls.userName.value),
        firstName: String(this.userForm.controls.firstName.value),
        lastName: String(this.userForm.controls.lastName.value),
        email: String(this.userForm.controls.email.value)
    })

    this.router.navigate(['/users']);
  }

  cancel(){
    this.router.navigate(['/users']);
  }
}
