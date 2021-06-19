import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { PremiumService } from '../../services/premium.service';

@Component({
  selector: 'app-premium-manage',
  templateUrl: './premium-manage.component.html',
  styleUrls: ['./premium-manage.component.scss']
})
export class PremiumManageComponent implements OnInit {

  constructor(private premiumService: PremiumService, private snack: SnackbarService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  doCancel(): void {
    this.premiumService.CancelPremium().then(async x => {
      if (x.success) {
        await this.authService.RefreshAccessToken();
        this.router.navigate(['premium']);
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "You are now unsubscribed.", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not subscribe.", AlertType.Error));
      }
    })
  }

}
