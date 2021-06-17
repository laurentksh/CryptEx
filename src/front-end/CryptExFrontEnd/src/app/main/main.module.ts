import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { ContactComponent } from './components/contact/contact.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MainRouting } from './main-routing';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ForbiddenComponent } from './components/forbidden/forbidden.component';
import { TranslateModule } from '@ngx-translate/core';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { WalletModule } from '../wallet/wallet.module';
import { StatusBadgeComponent } from './components/status-badge/status-badge.component';

@NgModule({
  declarations: [
    ContactComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    NotFoundComponent,
    ForbiddenComponent,
    UnauthorizedComponent,
    StatusBadgeComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    MainRouting,
    TranslateModule,
    WalletModule
  ],
  exports:[
    ContactComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    StatusBadgeComponent
  ]
})
export class MainModule { }