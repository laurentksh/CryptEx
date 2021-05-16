import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { ChangePasswordDto } from '../models/change-password-dto';
import { CurrencyUpdateDto } from '../models/currency-update-dto';
import { RequestPasswordChangeDto } from '../models/request-password-change-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: CustomHttpClientService) { }

  public async RequestPasswordChange(body: RequestPasswordChangeDto): Promise<ApiResult> {
    return this.http.Post("User/resetPassword", body);
  }

  public async ChangePassword(body: ChangePasswordDto): Promise<ApiResult> {
    return this.http.Post("User/changePassword", body);
  }

  public async UpdateLanguage(lang: string): Promise<ApiResult> {
    return this.http.Post("User/language", null, { params: new HttpParams().set("lang", lang) });
  }

  public async UpdateCurrency(selectedCurrency: string): Promise<ApiResult> {
    return this.http.Post("User/currency", { currency: selectedCurrency } as CurrencyUpdateDto);
  }
}
