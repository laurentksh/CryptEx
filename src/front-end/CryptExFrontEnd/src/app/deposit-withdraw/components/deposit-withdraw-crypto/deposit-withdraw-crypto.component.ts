import { Component, OnInit } from '@angular/core';
import { NgxQrcodeElementTypes, NgxQrcodeErrorCorrectionLevels } from '@techiediaries/ngx-qrcode';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserService } from 'src/app/user/services/user.service';
import { WalletType, WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { WalletService } from 'src/app/wallet/services/wallet.service';
import { CryptoDepositViewModel } from '../../models/crypto-deposit-view-model';
import { DepositWithdrawService } from '../../services/deposit-withdraw.service';

@Component({
  selector: 'app-deposit-withdraw-crypto',
  templateUrl: './deposit-withdraw-crypto.component.html',
  styleUrls: ['./deposit-withdraw-crypto.component.scss']
})
export class DepositWithdrawCryptoComponent implements OnInit {
  cryptos: WalletViewModel[];
  selectedCryptoId: string;
  amount: number = -1;
  depositVm: CryptoDepositViewModel;
  readonly elementType = NgxQrcodeElementTypes.IMG;
  readonly correctionLevel = NgxQrcodeErrorCorrectionLevels.HIGH;

  constructor(
    private depWitService: DepositWithdrawService,
    private walletService: WalletService,
    public userService: UserService,
    private snackBarService: SnackbarService) { }

  ngOnInit(): void {
    this.walletService.GetWalletList().then(x => {
      if (x.success) {
        this.cryptos = x.content.filter(x => x.type == WalletType.Crypto);
        this.selectedCryptoId = this.cryptos[0]?.id;
      } else {
        this.snackBarService.ShowSnackbar(new SnackBarCreate("Error", "Could not load cryptocurrency list.", AlertType.Error));
      }
    })
  }

  onDeposit(): void {
    this.depWitService.DepositCrypto(this.selectedCryptoId).then(x => {
      if (x.success) {
        this.depositVm = x.content;
        this.snackBarService.ShowSnackbar(new SnackBarCreate("Success", "Deposit address successfully generated.", AlertType.Success));
      } else {
        if (x.error.status == 400)
          this.snackBarService.ShowSnackbar(new SnackBarCreate("Error", "The cryptocurrency you selected is currently unavailable.", AlertType.Error));
        else
          this.snackBarService.ShowSnackbar(new SnackBarCreate("Error", "Could not deposit money.", AlertType.Error));
      }
    })
  }

  amountChanged(amount: number): void {
    this.amount = amount;
  }

  cryptoChanged($event: any): void {
    this.selectedCryptoId = $event.target.value;
  }
}
