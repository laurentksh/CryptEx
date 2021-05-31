import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './Components/home/home.component';
import { ContactComponent } from './Components/contact/contact.component';
import { HeaderComponent } from './Components/header/header.component';
import { FooterComponent } from './Components/footer/footer.component';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {MainRouting} from './main-routing';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { ForbiddenComponent } from './Components/forbidden/forbidden.component';
import { TranslateModule } from '@ngx-translate/core';
import { UnauthorizedComponent } from './Components/unauthorized/unauthorized.component';

@NgModule({
  declarations: [
    ContactComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    NotFoundComponent,
    ForbiddenComponent,
    UnauthorizedComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    MainRouting,
    TranslateModule
  ],
  exports:[
    ContactComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent
  ]
})
export class MainModule { }
