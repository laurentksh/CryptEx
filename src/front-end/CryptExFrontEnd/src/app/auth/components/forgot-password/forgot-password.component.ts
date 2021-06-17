import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { RequestPasswordChangeDto } from 'src/app/user/models/request-password-change-dto';
import { UserService } from 'src/app/user/services/user.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  changingPassword: boolean = false;
  dto: RequestPasswordChangeDto = {} as RequestPasswordChangeDto;

  constructor(private userService: UserService, private snack: SnackbarService, private router: Router) { }

  ngOnInit(): void {
  }

  doRequestPasswordChange(): void {
    this.changingPassword = true;
    this.userService.RequestPasswordChange(this.dto).then(x => {
      this.changingPassword = false;

      if (x.success) {
        const snackbar = new SnackBarCreate("Password change requested successfully.", "[DEBUG] Click 'Continue' to be redirected to the password change page.", AlertType.Success);
        snackbar.ShowCloseButton = false;
        snackbar.ShowContinueActionBtn = true;
        snackbar.OnContinueActionCallback = () => { this.router.navigate(["changePassword"], { queryParams: { "token": x.content.token }}) }
        this.snack.ShowSnackbar(snackbar);
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("An error occured.", "An error occured while trying to reset your password, please try again later.", AlertType.Error));
      }
    });
  }
}
