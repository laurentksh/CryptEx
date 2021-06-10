import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WalletsComponent } from './components/wallets/wallets.component';
import { AuthenticationGuard } from '../guards/authentication.guard';

const routes: Routes = [
  {
    path: 'wallets',
    component: WalletsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class WalletRoutingModule { }
