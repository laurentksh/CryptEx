import { Component, Input, OnInit } from '@angular/core';
import { WalletViewModel } from 'src/app/user/models/wallet-view-model';


@Component({
  selector: 'app-asset-card',
  templateUrl: './asset-card.component.html',
  styleUrls: ['./asset-card.component.scss']
})
export class AssetCardComponent implements OnInit {
  @Input() asset: WalletViewModel = {} as WalletViewModel;

  constructor() { }

  ngOnInit(): void {
  }

}
