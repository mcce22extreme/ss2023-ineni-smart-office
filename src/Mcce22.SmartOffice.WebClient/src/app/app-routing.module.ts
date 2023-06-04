import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultComponent } from './layouts/default/default.component';
import { DashboardComponent } from './modules/dashboard/dashboard.component';
import { UserListComponent } from './modules/users/user-list.component';
import { UserDetailComponent } from './modules/users/user-detail.component';
import { userResolver } from './modules/users/user.service';

const routes: Routes = [{
  path:'',
  component: DefaultComponent,
  children:[{
    path: '',
    component: DashboardComponent
  },
  {
    path: 'users',
    component: UserListComponent
  },
  {
    path: 'users/create',
    component: UserDetailComponent
  },
  {
    path: 'users/edit/:id',
    component: UserDetailComponent,
    resolve: {user: userResolver}
  }]
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
