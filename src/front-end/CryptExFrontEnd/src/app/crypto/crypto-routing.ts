import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CryptoComponent} from './Components/crypto/crypto.component';
import {BuySellComponent} from './Components/buy-sell/buy-sell.component';

const routes: Routes = [
  {
    path: 'crypto/:identifier',
    component: CryptoComponent
  },
  {
    path: 'buySell',
    component: BuySellComponent
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CryptoRouting { }
