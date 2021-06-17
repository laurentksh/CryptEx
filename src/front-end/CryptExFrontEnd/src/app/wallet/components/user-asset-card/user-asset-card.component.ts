import { Component, Input, OnInit } from '@angular/core';
import { UserWalletViewModel } from '../../models/user-wallet-view-model';


@Component({
  selector: 'app-user-asset-card',
  templateUrl: './user-asset-card.component.html',
  styleUrls: ['./user-asset-card.component.scss']
})
export class UserAssetCardComponent implements OnInit {
  @Input() asset: UserWalletViewModel;

  constructor() { }

  ngOnInit(): void {
    
  }

}
