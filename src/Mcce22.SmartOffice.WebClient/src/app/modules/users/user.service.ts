import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { lastValueFrom, of } from 'rxjs';
import { User } from './user.model';

export const userResolver: ResolveFn<User> = 
    (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {

        const id = route.paramMap.get('id');
        const service = inject(UserService);

        if(id){
            return service.getUser(id);
        }else{
            return service.newUser();
        }        
    };

@Injectable()
export class UserService {

    users:User[] = [{
        id:"demouser1",
        userName: "demo.user",
        firstName: "demo",
        lastName: "user",
        email: "demo.user@abc.com"
    },
    {
        id:"demouser2",
        userName: "demo.user",
        firstName: "demo",
        lastName: "user",
        email: "demo.user@abc.com"
    }];

    constructor(private httpClient: HttpClient){        
    }

    getUsers(){

        return of(this.users);
    }

    getUser(userId:string){

        const user = this.users.find(x => x.id == userId);
        return of(user as User);
    }

    newUser(){
        return of({
            id: '',
            userName: '',
            firstName: '',
            lastName: '',
            email: '',
        });
    }

    async saveUser(user:User) {

        if(user.id){

        const existingUser = await lastValueFrom(this.getUser(user.id));

        existingUser.userName = user.userName;
        existingUser.firstName = user.firstName;
        existingUser.lastName = user.lastName;
        existingUser.email = user.email;

        } else {
            this.users.push(user);
        }
    }

    deleteUser(userId:string):void{
        this.users = this.users.filter(x => x.id != userId);
    }
}