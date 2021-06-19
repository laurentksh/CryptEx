import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserService } from 'src/app/user/services/user.service';
import { WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { WalletService } from 'src/app/wallet/services/wallet.service';
import { AssetConversionLockDto } from '../../models/asset-conversion-lock-dto';
import { AssetConvertService } from '../../services/asset-convert.service';

@Component({
  selector: 'app-buy-sell',
  templateUrl: './buy-sell.component.html',
  styleUrls: ['./buy-sell.component.scss']
})
export class BuySellComponent implements OnInit {
  assets: WalletViewModel[];
  dto: AssetConversionLockDto = {} as AssetConversionLockDto;

  constructor(
    private walletService: WalletService,
    private service: AssetConvertService,
    private snack: SnackbarService,
    public user: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.walletService.GetWalletList().then(x => {
      if (x.success) {
        this.assets = x.content;

        this.dto.leftAssetId = this.assets.find(x => x.ticker == this.user.SelectedCurrency).id;
        this.dto.rightAssetId = this.assets.find(x => x.ticker == "BTC").id;
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load assets.", AlertType.Error));
      }
    })
  }

  leftAssetChanged($event: any): void {
    this.dto.leftAssetId = $event.target.value;
  }

  rightAssetChanged($event: any): void {
    this.dto.rightAssetId = $event.target.value;
  }

  doLock(): void {
    this.service.LockTransaction(this.dto).then(x => {
      if (x.success) {
        this.router.navigate(['/buy-sell/preview', x.content.id]);
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "Price locked, set an amount and confirm the transaction or cancel it.", AlertType.Success));
      } else {
        if (x.error.status == 400) {
          this.snack.ShowSnackbar(new SnackBarCreate("Insufficient funds", "You do not have enough funds to convert this asset.", AlertType.Error));
        } else {
          this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not convert assets.", AlertType.Error));
        }
      }
    });
  }
}
