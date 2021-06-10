import { Component, OnInit } from '@angular/core';
import { WalletViewModel } from 'src/app/user/models/wallet-view-model';
import { AssetCardComponent } from 'src/app/wallet/components/asset-card/asset-card.component';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
 
  assets: WalletViewModel[];

  constructor() { }

  ngOnInit(): void {
    this.assets = [
      { fullName: "Bitcoin", id:"1", ticker:"BTC", selectedCurrencyPair:{rate:36000}} as WalletViewModel
    ]
  }



}
