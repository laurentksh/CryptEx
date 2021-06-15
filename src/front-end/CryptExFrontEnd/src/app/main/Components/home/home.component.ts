import { Component, OnInit } from '@angular/core';
import { WalletType, WalletViewModel } from 'src/app/user/models/wallet-view-model';
import { AssetCardComponent } from 'src/app/wallet/components/asset-card/asset-card.component';
import { WalletService } from 'src/app/wallet/services/wallet.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
 
  assets: WalletViewModel[];

  constructor(private _walletService:WalletService) { }

  ngOnInit(): void {
    this._walletService.GetWalletList().then(x => {
      if (x.success){
        this.assets= x.content.filter(x => x.type == WalletType.Crypto);
      }
    })
  }



}
