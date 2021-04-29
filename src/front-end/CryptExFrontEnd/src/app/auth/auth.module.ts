import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './Components/login/login.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { RegisterComponent } from './Components/register/register.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {AuthRouting} from './auth-routing';



@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    RegisterComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    AuthRouting
  ]
})
export class AuthModule { }
