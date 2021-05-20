import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletsComponent } from './Components/wallets/wallets.component';
import { MyAccountComponent } from './Components/my-account/my-account.component';
import { DepositComponent } from './Components/deposit/deposit.component';
import {UserRouting} from './user-routing';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import { AssetCardComponent } from './Components/asset-card/asset-card.component';



@NgModule({
  declarations: [
    WalletsComponent,
    MyAccountComponent,
    DepositComponent,
    AssetCardComponent],
  imports: [
    CommonModule,
    UserRouting,
    FormsModule,
    BrowserModule,
  ]
})
export class UserModule { }
