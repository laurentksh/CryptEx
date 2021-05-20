import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AuthDto} from "../../models/auth-dto";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  user: AuthDto;
  error: HttpErrorResponse;

  constructor(private router: Router, )
  {
    this.user = {} as AuthDto;
  }

  ngOnInit(): void {
  }

}
