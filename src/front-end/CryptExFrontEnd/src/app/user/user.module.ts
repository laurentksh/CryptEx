import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletsComponent } from './Components/wallets/wallets.component';
import { MyAccountComponent } from './Components/my-account/my-account.component';
import { UserRouting } from './user-routing';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    WalletsComponent,
    MyAccountComponent
  ],
  imports: [
    CommonModule,
    UserRouting,
    FormsModule,
    BrowserModule,
    TranslateModule
  ]
})
export class UserModule { }
