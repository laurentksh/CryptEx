import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuySellComponent } from './Components/buy-sell/buy-sell.component';
import { CryptoComponent } from './Components/crypto/crypto.component';



@NgModule({
  declarations: [BuySellComponent, CryptoComponent],
  imports: [
    CommonModule
  ]
})
export class CryptoModule { }
