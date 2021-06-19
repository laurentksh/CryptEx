import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CryptoComponent } from './components/crypto/crypto.component';
import { BuySellComponent } from './components/buy-sell/buy-sell.component';
import { TransactionStatusPageComponent } from './components/transaction-status-page/transaction-status-page.component';
import { AuthenticationGuard } from '../guards/authentication.guard';
import { PreviewTransactionComponent } from './components/preview-transaction/preview-transaction.component';
import { TransactionHistoryComponent } from './components/transaction-history/transaction-history.component';

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
    component: TransactionStatusPageComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'buy-sell/preview/:id',
    component: PreviewTransactionComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'buy-sell/history',
    component: TransactionHistoryComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CryptoRouting { }
