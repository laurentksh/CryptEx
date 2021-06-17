import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { LogoutComponent } from './components/logout/logout.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
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
    component: ChangePasswordComponent
  },
  {
    path: 'forgotPassword',
    component: ForgotPasswordComponent,
    canActivate: [UnauthenticatedGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AuthRouting { }
