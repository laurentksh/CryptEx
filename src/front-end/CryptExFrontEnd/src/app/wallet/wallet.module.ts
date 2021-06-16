import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletRoutingModule } from './wallet-routing.module';
import { AssetCardComponent } from './components/asset-card/asset-card.component';
import { WalletsComponent } from './components/wallets/wallets.component';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    AssetCardComponent,
    WalletsComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    WalletRoutingModule,
    TranslateModule
  ],
  exports: [
    AssetCardComponent
  ]
})
export class WalletModule { }
