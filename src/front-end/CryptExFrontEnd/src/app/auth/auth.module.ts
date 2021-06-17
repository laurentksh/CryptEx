import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { RegisterComponent } from './components/register/register.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {AuthRouting} from './auth-routing';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    RegisterComponent,
    ChangePasswordComponent,
    ForgotPasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    AuthRouting,
    TranslateModule
  ]
})
export class AuthModule { }
