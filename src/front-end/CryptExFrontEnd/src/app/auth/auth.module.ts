import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './Components/login/login.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { RegisterComponent } from './Components/register/register.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';



@NgModule({
  declarations: [LoginComponent, LogoutComponent, RegisterComponent, ChangePasswordComponent],
  imports: [
    CommonModule
  ]
})
export class AuthModule { }
