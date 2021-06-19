import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuySellComponent } from './components/buy-sell/buy-sell.component';
import { CryptoComponent } from './components/crypto/crypto.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CryptoRouting } from './asset-convert-routing';
import { TranslateModule } from '@ngx-translate/core';
import { TransactionStatusPageComponent } from './components/transaction-status-page/transaction-status-page.component';
import { PreviewTransactionComponent } from './components/preview-transaction/preview-transaction.component';
import { TransactionHistoryComponent } from './components/transaction-history/transaction-history.component';



@NgModule({
  declarations: [
    BuySellComponent,
    CryptoComponent,
    TransactionStatusPageComponent,
    PreviewTransactionComponent,
    TransactionHistoryComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    CryptoRouting,
    TranslateModule
  ]
})
export class CryptoModule { }
