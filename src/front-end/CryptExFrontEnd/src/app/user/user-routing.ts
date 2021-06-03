import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from '../guards/authentication.guard';
import {MyAccountComponent} from './Components/my-account/my-account.component';
import {WalletsComponent} from './Components/wallets/wallets.component';

const routes: Routes = [
  {
    path: 'my-account',
    component: MyAccountComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'wallets',
    component: WalletsComponent,
    canActivate: [AuthenticationGuard]
  }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class UserRouting { }
