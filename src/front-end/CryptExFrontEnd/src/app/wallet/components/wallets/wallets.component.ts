import { Component, OnInit } from '@angular/core';
import { createChart, isBusinessDay } from 'lightweight-charts';
import { Subscription } from 'rxjs';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { CurrencyService } from 'src/app/main/services/currency.service';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { TotalsViewModel } from '../../models/totals-view-model';
import { UserWalletViewModel } from '../../models/user-wallet-view-model';
import { WalletService } from '../../services/wallet.service';

@Component({
  selector: 'app-wallets',
  templateUrl: './wallets.component.html',
  styleUrls: ['./wallets.component.scss']
})
export class WalletsComponent implements OnInit {
  sub: Subscription;
  wallets: UserWalletViewModel[];
  totals: TotalsViewModel;

  public hideEmptyWallets: boolean = false;

  constructor(private walletService: WalletService, private snack: SnackbarService, private curService: CurrencyService) { }

  ngOnInit(): void {
    this.loadData();
    this.sub = this.curService.OnCurrencyChange.subscribe(() => this.loadData());
  }

  loadData(): void {
    this.walletService.GetUserWallets().then(x => {
      if (x.success) {
        this.wallets = x.content;
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user wallets.", AlertType.Error));
      }
    });

    this.walletService.GetTotals().then(x => {
      if (x.success) {
        this.totals = x.content;

        this.hideEmptyWallets = x.content.accountTotal.amount != 0;
      } else
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not user data.", AlertType.Error));
    });
  }
}
