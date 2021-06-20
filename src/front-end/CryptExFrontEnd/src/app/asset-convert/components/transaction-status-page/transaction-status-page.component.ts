import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { PaymentStatus } from 'src/app/deposit-withdraw/models/deposit-view-model';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AssetConverssionViewModel } from '../../models/asset-converssion-view-model';
import { AssetConvertService } from '../../services/asset-convert.service';

@Component({
  selector: 'app-transaction-status-page',
  templateUrl: './transaction-status-page.component.html',
  styleUrls: ['./transaction-status-page.component.scss']
})
export class TransactionStatusPageComponent implements OnInit {
  transaction: AssetConverssionViewModel;
  paymentStatusRef = PaymentStatus;
  sub: Subscription;

  constructor(public service: AssetConvertService, private snack: SnackbarService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      const id = params["id"];

      if (id == null) {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load transaction", AlertType.Error));
        this.router.navigate(['buy-sell']);
        return;
      }

      //this.service.BeginSignalR(id).then();
      /*this.service.transaction = {
        amount: 5.5,
        id: 'TEST-91230912039',
        status: PaymentStatus.notProcessed,
        pair: {
          rate: 50,
          left: {
            ticker: 'BTC'
          },
          right: {
            ticker: 'USD'
          }
        }
      } as AssetConvertViewModel;*/
      this.service.GetTransaction(id).then(x => this.transaction = x.content);

      setTimeout(() => {
        this.transaction.status = PaymentStatus.pending
      }, 4000);
      setTimeout(() => {
        this.transaction.status = PaymentStatus.success
      }, 8000);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  getStatusText(): string {
    switch (this.transaction.status) {
      case PaymentStatus.notProcessed:
        return 'InProgress';
      case PaymentStatus.failed:
        return 'Failed';
      case PaymentStatus.success:
        return 'Done';
      case PaymentStatus.pending:
        return 'InProgress';
      default:
        break;
    }
  }

  getPercentage(): string {
    if (this.transaction == null)
      return '-';
    
    switch (this.transaction.status) {
      case PaymentStatus.notProcessed:
        return '25%';
      case PaymentStatus.failed:
        return '-%';
      case PaymentStatus.success:
        return '100%';
      case PaymentStatus.pending:
        return '50%';
      default:
        break;
    }
  }
}
