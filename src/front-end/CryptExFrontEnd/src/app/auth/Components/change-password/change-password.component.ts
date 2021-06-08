import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { ChangePasswordDto } from 'src/app/user/models/change-password-dto';
import { UserService } from 'src/app/user/services/user.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  changingPassword: boolean = false;
  passwordValidation: string;
  dto: ChangePasswordDto = {} as ChangePasswordDto;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private snack: SnackbarService,
    private route: ActivatedRoute,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => this.handleParams(params));
  }

  handleParams(params: Params): void {
    const resetToken = params["token"];

    if (resetToken == null && !this.IsAuthenticated()) {
      this.router.navigate(["home"]);
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Missing reset token.", AlertType.Error, 10000));
    }

    this.dto.token = resetToken;
  }

  IsAuthenticated(): boolean {
    return this.authService.IsAuthenticated;
  }

  doChangePassword(): void {
    this.changingPassword = true;
    if (this.dto.newPassword != this.passwordValidation || (this.stringIsNullOrEmpty(this.dto.newPassword) || this.stringIsNullOrEmpty(this.passwordValidation))) {
      debugger;
      this.changingPassword = false;
      this.snack.ShowSnackbar(new SnackBarCreate("Passwords don't match", "New password and confirm password don't match.", AlertType.Error, 5000));
      return;
    }

    this.changePasswordAsync();
  }

  async changePasswordAsync(): Promise<void> {
    if (this.IsAuthenticated && this.dto.token == null) {
      const changeResult = await this.userService.RequestPasswordChange({} as ChangePasswordDto);
      if (!changeResult.success) {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not change password, please try again later.", AlertType.Error));
        return;
      }

      this.dto.token = changeResult.content.token;
    }

    const result = await this.userService.ChangePassword(this.dto);
    this.changingPassword = false;
    if (result.success) {
      this.snack.ShowSnackbar(new SnackBarCreate("Password changed successfully !", "Your password has been changed.", AlertType.Success));
    } else {
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "An error occured.", AlertType.Error))
    }
  }

  stringIsNullOrEmpty(value: string): boolean {
    return value == null || value == "" || !value.trim();
  }
}
