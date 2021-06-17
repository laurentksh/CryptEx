import { Component, OnInit } from '@angular/core';
import { createChart, isBusinessDay } from 'lightweight-charts';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { TotalViewModel } from '../../models/total-view-model';
import { UserWalletViewModel } from '../../models/user-wallet-view-model';
import { WalletService } from '../../services/wallet.service';

@Component({
  selector: 'app-wallets',
  templateUrl: './wallets.component.html',
  styleUrls: ['./wallets.component.scss']
})
export class WalletsComponent implements OnInit {
  wallets: UserWalletViewModel[];
  total: TotalViewModel;
  totalFiat: TotalViewModel;
  totalCrypto: TotalViewModel;

  constructor(private walletService: WalletService, private snack: SnackbarService) { }

  ngOnInit(): void {
    this.walletService.GetUserWallets().then(x => {
      if (x.success) {
        this.wallets = x.content;
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user wallets.", AlertType.Error));
      }
    });

    this.walletService.GetTotal().then(x => {
      if (x.success)
        this.total = x.content;
      else
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not user data.", AlertType.Error));
    });

    this.walletService.GetTotalFiat().then(x => {
      if (x.success)
        this.totalFiat = x.content;
      else
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not user data.", AlertType.Error));
    });

    this.walletService.GetTotalCrypto().then(x => {
      if (x.success)
        this.totalCrypto = x.content;
      else
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not user data.", AlertType.Error));
    });
  }
}
