import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserService } from 'src/app/user/services/user.service';
import { AuthDto } from '../../models/auth-dto';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  authDto: AuthDto = {} as AuthDto;
  loggingIn: boolean = false;

  constructor(private authService: AuthService, private userService: UserService, private router: Router, private snackService: SnackbarService) { }

  ngOnInit(): void {
  }

  doLogin(): void {
    if (this.loggingIn) //Prevent spam
      return;
    
    this.loggingIn = true;
    this.authService.Authenticate(this.authDto).then(async x => {
      if (x.success) {
        await this.userService.RefreshUser();
        this.loggingIn = false;
        this.snackService.ClearSnackBars();
        this.router.navigate(["my-account"])
        this.snackService.ShowSnackbar(new SnackBarCreate("Successfully logged in", "Welcome back !", AlertType.Success, 5000));
      } else {
        this.loggingIn = false;
        this.snackService.ShowSnackbar(new SnackBarCreate("Could not login into your account", "Please check your credentials are correct.", AlertType.Error, 5000));
      }
    })
  }
}
