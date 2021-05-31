import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './Components/register/register.component';
import { LoginComponent } from './Components/login/login.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { ForgotPasswordComponent } from './Components/forgot-password/forgot-password.component';
import { AuthenticationGuard } from '../guards/authentication.guard';
import { UnauthenticatedGuard } from '../guards/unauthenticated.guard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [UnauthenticatedGuard]
  },
  {
    path: 'signup',
    component: RegisterComponent,
    canActivate: [UnauthenticatedGuard]
  },
  {
    path: 'logout',
    component: LogoutComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'changePassword',
    component: ChangePasswordComponent,
    canActivate: [AuthenticationGuard]
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
