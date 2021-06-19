import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserService } from 'src/app/user/services/user.service';
import { WalletType, WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { WalletService } from 'src/app/wallet/services/wallet.service';
import { CurrencyService } from '../../services/currency.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  sub: Subscription;
  assets: WalletViewModel[];

  constructor(private _walletService: WalletService, private snack: SnackbarService, public userService: UserService, private curService: CurrencyService) { }

  ngOnInit(): void {
    this.loadCurrencies();
    this.sub = this.curService.OnCurrencyChange.subscribe(() => this.loadCurrencies())
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  loadCurrencies(): void {
    this._walletService.GetWalletList().then(x => {
      if (x.success){
        this.assets = x.content.filter(x => x.type == WalletType.Crypto);
      } else {
        this.assets = []
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load crypto-currencies data.", AlertType.Error));
      }
    });
  }
}
