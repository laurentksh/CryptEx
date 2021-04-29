import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {DepositComponent} from './Components/deposit/deposit.component';
import {MyAccountComponent} from './Components/my-account/my-account.component';
import {WalletsComponent} from './Components/wallets/wallets.component';

const routes: Routes = [
  {
    path: 'deposit',
    component: DepositComponent
  },
  {
    path: 'myAccount',
    component: MyAccountComponent
  },
  {
    path: 'wallets',
    component: WalletsComponent
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class UserRouting { }
