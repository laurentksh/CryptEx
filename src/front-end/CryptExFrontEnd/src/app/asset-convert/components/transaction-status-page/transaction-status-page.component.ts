import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { PaymentStatus } from 'src/app/deposit-withdraw/models/deposit-view-model';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AssetConvertViewModel } from '../../models/asset-convert-view-model';
import { AssetConvertService } from '../../services/asset-convert.service';

@Component({
  selector: 'app-transaction-status-page',
  templateUrl: './transaction-status-page.component.html',
  styleUrls: ['./transaction-status-page.component.scss']
})
export class TransactionStatusPageComponent implements OnInit {
  transaction: AssetConvertViewModel;
  paymentStatusRef = PaymentStatus;

  constructor(public service: AssetConvertService, private snack: SnackbarService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params["id"];

      if (id == null) {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load transaction", AlertType.Error));
        return;
      }

      //this.service.BeginSignalR(id).then()
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
      }, 10000);
    });
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
