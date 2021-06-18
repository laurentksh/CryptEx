import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CryptoComponent } from './components/crypto/crypto.component';
import { BuySellComponent } from './components/buy-sell/buy-sell.component';
import { TransactionStatusPageComponent } from './components/transaction-status-page/transaction-status-page.component';
import { AuthenticationGuard } from '../guards/authentication.guard';

const routes: Routes = [
  {
    path: 'crypto/:identifier',
    component: CryptoComponent
  },
  {
    path: 'buy-sell',
    component: BuySellComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'buy-sell/transaction/:id',
    component: TransactionStatusPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CryptoRouting { }
