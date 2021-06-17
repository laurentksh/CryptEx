import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationGuard } from '../guards/authentication.guard';
import { MyAccountComponent } from './components/my-account/my-account.component';

const routes: Routes = [
  {
    path: 'my-account',
    component: MyAccountComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class UserRouting { }
