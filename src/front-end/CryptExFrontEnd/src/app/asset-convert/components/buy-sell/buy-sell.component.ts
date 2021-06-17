import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserService } from 'src/app/user/services/user.service';
import { WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { WalletService } from 'src/app/wallet/services/wallet.service';
import { AssetConvertDto } from '../../models/asset-convert-dto';
import { AssetConvertService } from '../../services/asset-convert.service';

@Component({
  selector: 'app-buy-sell',
  templateUrl: './buy-sell.component.html',
  styleUrls: ['./buy-sell.component.scss']
})
export class BuySellComponent implements OnInit {
  assets: WalletViewModel[];
  dto: AssetConvertDto = {} as AssetConvertDto;
  amount: number = -1;

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
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load assets.", AlertType.Error));
      }
    })
  }

  amountChanged(amount: number): void {
    this.dto.amount = amount;
  }

  leftAssetChanged($event: any): void {
    this.dto.leftAssetId = $event.target.value;
  }

  rightAssetChanged($event: any): void {
    this.dto.rightAssetId = $event.target.value;
  }

  doConvert(): void {
    this.service.Convert(this.dto).then(x => {
      if (x.success) {
        this.router.navigate(['/buy-sell/transaction', x.content]);
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "The conversion has begun. Watch as it happens in real time !", AlertType.Success));
      } else {
        if (x.error.status == 400) {
          this.snack.ShowSnackbar(new SnackBarCreate("Insufficient funds", "You do not have enough funds to convert this asset.", AlertType.Error));
        } else {
          this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not convert assets.", AlertType.Error));
        }
      }
    })
  }
}
