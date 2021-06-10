import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletRoutingModule } from './wallet-routing.module';
import { AssetCardComponent } from './components/asset-card/asset-card.component';
import { WalletsComponent } from './components/wallets/wallets.component';



@NgModule({
  declarations: [
    AssetCardComponent,
    WalletsComponent
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
