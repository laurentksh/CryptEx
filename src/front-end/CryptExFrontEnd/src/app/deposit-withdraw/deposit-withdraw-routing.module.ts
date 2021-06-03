import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepositWithdrawFiatComponent } from './components/deposit-withdraw-fiat/deposit-withdraw-fiat.component';
import { DepositWithdrawCryptoComponent } from './components/deposit-withdraw-crypto/deposit-withdraw-crypto.component';
import { DepositWithdrawHomeComponent } from './components/deposit-withdraw-home/deposit-withdraw-home.component';
import { AuthenticationGuard } from '../guards/authentication.guard';


const routes: Routes = [
  {
    path: 'deposit-withdraw',
    component: DepositWithdrawHomeComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'deposit-withdraw/fiat',
    component: DepositWithdrawFiatComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'deposit-withdraw/crypto',
    component: DepositWithdrawCryptoComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class DepositWithdrawRoutingModule { }
