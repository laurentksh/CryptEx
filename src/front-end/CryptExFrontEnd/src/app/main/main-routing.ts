import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './Components/home/home.component';
import {ContactComponent} from './Components/contact/contact.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { ForbiddenComponent } from './Components/forbidden/forbidden.component';
import { UnauthorizedComponent } from './Components/unauthorized/unauthorized.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'contact',
    component: ContactComponent
  },
  {
    path: 'notfound',
    component: NotFoundComponent
  },
  {
    path: "unauthorized",
    component: UnauthorizedComponent
  },
  {
    path: 'forbidden',
    component: ForbiddenComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class MainRouting { }
