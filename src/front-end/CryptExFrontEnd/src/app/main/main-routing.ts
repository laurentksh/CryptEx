import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {ContactComponent} from './components/contact/contact.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ForbiddenComponent } from './components/forbidden/forbidden.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';

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
