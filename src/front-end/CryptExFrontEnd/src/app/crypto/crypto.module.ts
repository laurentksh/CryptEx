import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuySellComponent } from './Components/buy-sell/buy-sell.component';
import { CryptoComponent } from './Components/crypto/crypto.component';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {CryptoRouting} from './crypto-routing';



@NgModule({
  declarations: [
    BuySellComponent,
    CryptoComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    CryptoRouting
  ]
})
export class CryptoModule { }
