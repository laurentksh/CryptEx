import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { AuthDto } from '../models/auth-dto';
import { AuthViewModel } from '../models/auth-view-model';
import { CreateUserDto } from '../models/create-user-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper = new JwtHelperService();

  constructor(private http: CustomHttpClientService) { }

  public get IsAuthenticated(): boolean {
    return this.JWToken != null && !this.jwtHelper.isTokenExpired(this.JWToken) && this.jwtHelper.decodeToken(this.JWToken)
  }

  public get IsExpired(): boolean {
    return this.jwtHelper.isTokenExpired(this.JWToken);
  }

  public get JWTokenParsed(): any {
    return this.jwtHelper.decodeToken(this.JWToken);
  }

  public get JWToken(): string {
    return localStorage.getItem("accesstoken");
  }

  public set JWToken(value: string) {
    localStorage.setItem("accesstoken", value);
  }

  public async Authenticate(auth: AuthDto): Promise<ApiResult<AuthViewModel>> {
    const result = await this.http.Post<AuthViewModel>("Auth", auth);

    if (result.success) {
      this.JWToken = result.content.jwToken;
    }

    return result;
  }

  public async Signup(account: CreateUserDto): Promise<ApiResult<AuthViewModel>> {
    const result = await this.http.Post<AuthViewModel>("Auth/signup", account);

    if (result.success) {
      this.JWToken = result.content.jwToken;
    }

    return result;
  }
}
