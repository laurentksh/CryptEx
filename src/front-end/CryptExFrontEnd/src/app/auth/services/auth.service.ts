import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { AuthDto } from '../models/auth-dto';
import { AuthViewModel } from '../models/auth-view-model';
import { CreateUserDto } from '../models/create-user-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: CustomHttpClientService) { }

  public async Authenticate(auth: AuthDto): Promise<ApiResult<AuthViewModel>> {
    return this.http.Post<AuthViewModel>("Auth", auth);
  }

  public async Signup(account: CreateUserDto): Promise<ApiResult<AuthViewModel>> {
    return this.http.Post<AuthViewModel>("Auth/signup", account);
  }
}
