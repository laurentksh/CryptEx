import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletRoutingModule } from './wallet-routing.module';
import { AssetCardComponent } from './components/asset-card/asset-card.component';



@NgModule({
  declarations: [
    AssetCardComponent
  ],
  imports: [
    CommonModule,
    WalletRoutingModule
  ],
  exports: [
    AssetCardComponent
  ]
})
export class WalletModule { }
