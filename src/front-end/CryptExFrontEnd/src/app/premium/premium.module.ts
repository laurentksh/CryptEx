import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PremiumRouting } from "./premium-routing";
import { PremiumHomeComponent } from './components/premium-home/premium-home.component';
import { TranslateModule } from '@ngx-translate/core';
import { PremiumManageComponent } from './components/premium-manage/premium-manage.component';
import { PremiumPayComponent } from './components/premium-pay/premium-pay.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';



@NgModule({
  declarations: [
    PremiumHomeComponent,
    PremiumManageComponent,
    PremiumPayComponent
  ],
  imports: [
    CommonModule,
    PremiumRouting,
    BrowserModule,
    FormsModule,
    TranslateModule
  ]
})
export class PremiumModule { }
