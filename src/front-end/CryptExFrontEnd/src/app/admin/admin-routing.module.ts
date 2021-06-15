import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from '../guards/admin.guard';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { DepositsComponent } from './components/deposits/deposits.component';
import { HomeComponent } from './components/home/home.component';
import { PendingBankAccountsComponent } from './components/pending-bank-accounts/pending-bank-accounts.component';
import { SearchUserComponent } from './components/search-user/search-user.component';

const routes: Routes = [
  {
    path: 'admin',
    component: AdminDashboardComponent,
    canActivate: [AdminGuard],
    children: [
      {
        path: '',
        component: HomeComponent
      },
      {
        path: 'search-user',
        component: SearchUserComponent
      },
      {
        path: 'deposits',
        component: DepositsComponent
      },
      {
        path: 'bank-accounts',
        component: PendingBankAccountsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
