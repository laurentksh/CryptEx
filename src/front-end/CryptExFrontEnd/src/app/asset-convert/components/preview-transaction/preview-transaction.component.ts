import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { WalletType } from 'src/app/wallet/models/wallet-view-model';
import { AssetConversionLockViewModel } from '../../models/asset-conversion-lock-view-model';
import { AssetConverssionDto } from '../../models/asset-converssion-dto';
import { AssetConvertService } from '../../services/asset-convert.service';

@Component({
  selector: 'app-preview-transaction',
  templateUrl: './preview-transaction.component.html',
  styleUrls: ['./preview-transaction.component.scss']
})
export class PreviewTransactionComponent implements OnInit {
  dto: AssetConverssionDto = {} as AssetConverssionDto;
  lock: AssetConversionLockViewModel;
  sub: Subscription;

  constructor(private service: AssetConvertService, private snack: SnackbarService, private router: Router , private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      const id = params["id"];

      if (id == "test") {
        this.lock = {
          id: "TESTLOCK",
          expirationUtc: "2021-06-19T18:56:00",
          pair: {
            left: {
              ticker: "USD",
              fullName: "US Dollar",
              id: "TESTUSD",
              type: WalletType.Fiat,
              selectedCurrencyPair: null
            },
            right: {
              id: "TESTBTC",
              ticker: "BTC",
              fullName: "Bitcoin",
              selectedCurrencyPair: null,
              type: WalletType.Crypto
            },
            rate: 0.000032
          }
        }
        return;
      }

      if (id == null) {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load transaction lock.", AlertType.Error));
        this.router.navigate(['buy-sell']);
        return;
      }

      this.loadLock(id);
    });
  }

  loadLock(id: string): void {
    this.service.GetTransactionLock(id).then(x => {
      if (x.success) {
        this.lock = x.content;
      } else {
        this.router.navigate(['buy-sell']);
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "This price lock has expired, please try again.", AlertType.Error));
      }
    })
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  amountChanged(amount: number): void {
    this.dto.amount = amount;
  }

  getResultPrice(): number {
    return Math.round(((this.dto.amount * this.lock.pair.rate) + Number.EPSILON) * 100000) / 100000
  }

  doConvert(): void {
    this.service.Convert(this.dto).then(x => {
      if (x.success) {
        this.router.navigate(['/buy-sell/transaction', x.content]);
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "The conversion has begun. Watch as it happens in real time !", AlertType.Success));
      } else {
        if (x.error.status == 400) {
          this.snack.ShowSnackbar(new SnackBarCreate("Insufficient funds", "You do not have enough funds to convert this asset.", AlertType.Error));
        } else if (x.error.status == 404) {
          this.router.navigate(['buy-sell']);
          this.snack.ShowSnackbar(new SnackBarCreate("Error", "This price lock has expired, please try again.", AlertType.Error));
        } else {
          this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not convert assets.", AlertType.Error));
        }
      }
    });
  }

  doCancel(): void {
    this.service.RemoveTransactionLock(this.lock.id);
    this.router.navigate(['buy-sell']);
    this.snack.ShowSnackbar(new SnackBarCreate("Success", "Transaction successfully cancelled.", AlertType.Success));
  }
}
