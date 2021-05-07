import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {RegisterComponent} from './Components/register/register.component';
import {LoginComponent} from './Components/login/login.component';
import {ChangePasswordComponent} from './Components/change-password/change-password.component';
import {LogoutComponent} from './Components/logout/logout.component';
import { ForgotPasswordComponent } from './Components/forgot-password/forgot-password.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'logout',
    component: LogoutComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'changePassword',
    component: ChangePasswordComponent
  },
  {
    path: 'forgotPassword',
    component: ForgotPasswordComponent
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AuthRouting { }
