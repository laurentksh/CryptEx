import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AuthDto} from "../../models/auth-dto";
import {HttpErrorResponse} from "@angular/common/http";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  user: AuthDto;
  error: HttpErrorResponse;

  constructor(private router: Router, private _service: AuthService)
  {
    this.user = {} as AuthDto;
  }

  ngOnInit(): void {
    if (this._service.IsAuthenticated)
    {
      this.router.navigate(['home']);
    }
  }

  login(): void
  {
    this._service.Authenticate(this.user).then(x => {
      if (x.success)
      {
        this.router.navigate(['home']);
      }
      else
      {
        this.error = x.error
      }
    });
  }

}
