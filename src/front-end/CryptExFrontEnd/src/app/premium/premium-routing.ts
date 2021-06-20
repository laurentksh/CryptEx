import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from '../guards/authentication.guard';
import { PremiumGuard } from '../guards/premium.guard';
import { PremiumHomeComponent } from './components/premium-home/premium-home.component';
import { PremiumManageComponent } from './components/premium-manage/premium-manage.component';
import { PremiumPayComponent } from './components/premium-pay/premium-pay.component';

const routes: Routes = [
  {
    path: 'premium',
    component: PremiumHomeComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'premium/pay',
    component: PremiumPayComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: 'premium/manage',
    component: PremiumManageComponent,
    canActivate: [AuthenticationGuard, PremiumGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class PremiumRouting { }
