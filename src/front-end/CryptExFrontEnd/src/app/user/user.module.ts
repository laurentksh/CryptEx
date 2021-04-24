import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletsComponent } from './Components/wallets/wallets.component';
import { MyAccountComponent } from './Components/my-account/my-account.component';
import { DepositComponent } from './Components/deposit/deposit.component';



@NgModule({
  declarations: [WalletsComponent, MyAccountComponent, DepositComponent],
  imports: [
    CommonModule
  ]
})
export class UserModule { }
