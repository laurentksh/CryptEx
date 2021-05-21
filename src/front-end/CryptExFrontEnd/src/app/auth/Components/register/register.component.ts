import { Component, OnInit } from '@angular/core';
import {CreateUserDto} from "../../models/create-user-dto";
import {AuthService} from "../../services/auth.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  user: CreateUserDto;
  error: HttpErrorResponse;

  constructor(private _service: AuthService, private router: Router)
  {
    this.user = {} as CreateUserDto;
  }

  ngOnInit(): void {
    if (this._service.IsAuthenticated)
    {
      this.router.navigate(['home']);
    }
  }

  createUser(): void
  {
    this._service.Signup(this.user).then(x => {
      if (x.success)
      {
        this.router.navigate(['home']);
      }
      else
        {
          this.error = x.error
        }
    })
  }

}
