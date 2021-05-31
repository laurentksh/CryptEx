import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepositWithdrawFiatComponent } from './components/deposit-withdraw-fiat/deposit-withdraw-fiat.component';
import { DepositWithdrawRoutingModule } from './deposit-withdraw-routing.module';
import { DepositWithdrawCryptoComponent } from './components/deposit-withdraw-crypto/deposit-withdraw-crypto.component';
import { DepositWithdrawHomeComponent } from './components/deposit-withdraw-home/deposit-withdraw-home.component';
import { DepositComponent } from './components/deposit/deposit.component';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    DepositWithdrawFiatComponent,
    DepositWithdrawCryptoComponent,
    DepositWithdrawHomeComponent,
    DepositComponent
  ],
  imports: [
    CommonModule,
    DepositWithdrawRoutingModule,
    TranslateModule
  ]
})
export class DepositWithdrawModule { }
