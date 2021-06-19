import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletRoutingModule } from './wallet-routing.module';
import { AssetCardComponent } from './components/asset-card/asset-card.component';
import { WalletsComponent } from './components/wallets/wallets.component';
import { BrowserModule } from '@angular/platform-browser';
import { TranslateModule } from '@ngx-translate/core';
import { UserAssetCardComponent } from './components/user-asset-card/user-asset-card.component';
import { FormsModule } from '@angular/forms';
import { PercentageCardComponent } from './components/percentage-card/percentage-card.component';



@NgModule({
  declarations: [
    AssetCardComponent,
    UserAssetCardComponent,
    WalletsComponent,
    PercentageCardComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule,
    WalletRoutingModule,
    TranslateModule
  ],
  exports: [
    AssetCardComponent
  ]
})
export class WalletModule { }
