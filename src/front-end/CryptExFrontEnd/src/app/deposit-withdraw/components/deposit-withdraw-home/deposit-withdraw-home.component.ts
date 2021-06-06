import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { CryptoDepositViewModel } from '../../models/crypto-deposit-view-model';
import { DepositViewModel } from '../../models/deposit-view-model';
import { FiatDepositViewModel } from '../../models/fiat-deposit-view-model';
import { DepositWithdrawService } from '../../services/deposit-withdraw.service';

@Component({
  selector: 'app-deposit-withdraw-home',
  templateUrl: './deposit-withdraw-home.component.html',
  styleUrls: ['./deposit-withdraw-home.component.scss']
})
export class DepositWithdrawHomeComponent implements OnInit {
  constructor(public depWithService: DepositWithdrawService, private snackbar: SnackbarService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => this.handleParams(params));
    this.depWithService.BeginSignalR().then(x => {
      if (!x.success) {
        this.snackbar.ShowSnackbar(new SnackBarCreate("Deposits history couldn't be loaded", "An error occured while loading the deposit history.", AlertType.Error));
      }
    })
  }

  ngOnDestroy(): void {
    this.depWithService.EndSignalR();
  }

  private handleParams(params: Params): void {
    const fiatDepositStatus = params["fiatDepositStatus"];
    const cryptoDepositStatus = params["cryptoDepositStatus"];
    
    if (fiatDepositStatus != null) {
      this.router.navigate([], { queryParams: { 'fiatDepositStatus': null }});
      
      if (fiatDepositStatus == "success") {
        this.snackbar.ShowSnackbar(new SnackBarCreate(
          "Success",
          "Your deposit has been registered. It will appear in your deposit history and be soon processed.",
          AlertType.Success,
          10000
        ));
      } else if (fiatDepositStatus == "cancelled") {
        this.snackbar.ShowSnackbar(new SnackBarCreate(
          "Cancelled",
          "Your deposit has been cancelled. Please try again later or contact us if the issue persist.",
          AlertType.Error,
          10000
        ));
      }
    }

    if (cryptoDepositStatus != null) {
      this.router.navigate([], { queryParams: { 'cryptoDepositStatus': null }});

      if (cryptoDepositStatus == "success") {
        this.snackbar.ShowSnackbar(new SnackBarCreate(
          "Success",
          "Your deposit has been registered. It will appear in your deposit history and be soon processed.",
          AlertType.Success,
          10000
        ));
      } else if (cryptoDepositStatus == "cancelled") {
        this.snackbar.ShowSnackbar(new SnackBarCreate(
          "Cancelled",
          "Your deposit has been cancelled. Please try again later or contact us if the issue persist.",
          AlertType.Error,
          10000
        ));
      }
    }
  }

}