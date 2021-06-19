import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { BuyPremiumDto } from '../../models/buy-premium-dto';
import { PremiumService } from '../../services/premium.service';

@Component({
  selector: 'app-premium-pay',
  templateUrl: './premium-pay.component.html',
  styleUrls: ['./premium-pay.component.scss']
})
export class PremiumPayComponent implements OnInit {
  dto: BuyPremiumDto = {} as BuyPremiumDto;

  constructor(private premiumService: PremiumService, private snack: SnackbarService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  doPay(): void {
    this.premiumService.BuyPremium(this.dto).then(async x => {
      if (x.success) {
        await this.authService.RefreshAccessToken();
        this.router.navigate(['premium']);
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "You are now subscribed, thank you !", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not subscribe.", AlertType.Error));
      }
    })
  }
}
