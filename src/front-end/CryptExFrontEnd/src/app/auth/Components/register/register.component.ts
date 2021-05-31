import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { CreateUserDto } from '../../models/create-user-dto';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerDto: CreateUserDto = {} as CreateUserDto;
  registering: boolean = false;

  constructor(private authService: AuthService, private router: Router, private snackService: SnackbarService) { }

  ngOnInit(): void {
  }

  doRegister(): void {
    if (this.registering) //Prevent spam
      return;
    
    this.registering = true;
    this.authService.Signup(this.registerDto).then(x => {
      this.registering = false;

      if (x.success) {
        this.snackService.ClearSnackBars();
        this.router.navigate(["my-account"]);
        this.snackService.ShowSnackbar(new SnackBarCreate("Account created successfully", "Welcome to CryptEx !", AlertType.Success, 5000));
      } else {
        this.snackService.ShowSnackbar(new SnackBarCreate("Could not create account", "Make sure you filled all fields correctly !", AlertType.Error, 5000));
      }
    })
  }

}
