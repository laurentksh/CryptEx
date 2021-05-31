import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private snackService: SnackbarService) { }

  ngOnInit(): void {
  }

  doLogout(): void {
    this.authService.Logout();
    this.router.navigate(["/home"]);
    this.snackService.ShowSnackbar(new SnackBarCreate("Successfully logged out", "You have been safely logged out of your account !", AlertType.Success, 5000));
  }
}
