import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { SnackBar, SnackBarCreate, AlertType } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { StripeService } from 'src/app/stripe/services/stripe.service';
import { UserService } from 'src/app/user/services/user.service';
import { BankAccountStatus, BankAccountViewModel } from '../../models/bank-account-view-model';
import { DepositWithdrawService } from '../../services/deposit-withdraw.service';

@Component({
  selector: 'app-deposit-withdraw-fiat',
  templateUrl: './deposit-withdraw-fiat.component.html',
  styleUrls: ['./deposit-withdraw-fiat.component.scss']
})
export class DepositWithdrawFiatComponent implements OnInit {
  amount: number = -1;
  bankAccount: BankAccountViewModel;
  bankAccountStatusRef = BankAccountStatus;

  constructor(
    private depWitService: DepositWithdrawService,
    private stripe: StripeService,
    public userService: UserService,
    private authService: AuthService,
    private snackBarService: SnackbarService) { }

  ngOnInit(): void {
    this.userService.GetIban().then(x => {
      if (x.success) {
        this.bankAccount = x.content;
      } else {
        this.snackBarService.ShowSnackbar(new SnackBarCreate("Error", "Could not get bank account information.", AlertType.Error));
      }
    })
  }

  isPremium(): boolean {
    return this.authService.HasClaim("premium");
  }

  onDeposit(): void {
    this.depWitService.DepositFiat(this.amount).then(x => {
      if (x.success) {
        this.stripe.RedirectToCheckout(x.content.sessionId);
      } else {
        this.snackBarService.ShowSnackbar(new SnackBarCreate("Could not create payment session.", x.error.message, AlertType.Error));
      }
    })
  }

  onWithdraw(): void {
    this.depWitService.WithdrawFiat(this.amount).then(x => {
      if (x.success) {
        this.snackBarService.ShowSnackbar(new SnackBarCreate(
          "Withdraw requested",
          "Your withdrawal request has been submitted successfully, you will receive your money in 3-5 business days.",
          AlertType.Success));
      } else {
        if (x.error.status == 400) {
          this.snackBarService.ShowSnackbar(new SnackBarCreate(
            "Insufficient funds",
            "You do not have enough funds to withdraw that amount.",
            AlertType.Error
          ));
        } else if (x.error.status == 403) {
          this.snackBarService.ShowSnackbar(new SnackBarCreate(
            "Bank account not approved",
            "Your bank account hasn't been approved yet or has been refused. Please try again later",
            AlertType.Error
          ));
        } else {
          this.snackBarService.ShowSnackbar(new SnackBarCreate(
            "An error occured",
            `An unknown error occured. Please try again later. (${x.error.status})`,
            AlertType.Error
          ))
        }
      }
    })
  }

  amountChanged(amount: number): void {
    this.amount = amount;
  }
}
