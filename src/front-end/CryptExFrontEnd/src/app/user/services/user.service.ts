import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { ChangePasswordDto } from '../models/change-password-dto';
import { Currency } from '../models/currency';
import { Language } from '../models/language';
import { RequestPasswordChangeDto } from '../models/request-password-change-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public languages: Language[];
  public currencies: Currency[];
  public selectedLang = "en-us";
  public selectedCurrency = "usd";

  constructor(private http: CustomHttpClientService) {
    this.languages = [
      { Id: "en-us", DisplayText: "English" },
      { Id: "fr-fr", DisplayText: "Français" },
      { Id: "de-de", DisplayText: "Deutsch" }
    ];

    this.currencies = [
      { Id: "usd", DisplayText: "US Dollar (USD)" },
      { Id: "chf", DisplayText: "Swiss Franc (CHF)" },
      { Id: "eur", DisplayText: "Euro (EUR)" },
      { Id: "gbp", DisplayText: "British Pound (GBP)" },
      { Id: "cad", DisplayText: "Canadian Dollar (CAD)" },
      { Id: "aud", DisplayText: "Australian Dollar (AUD)" },
      { Id: "jpy", DisplayText: "Japan Yen (JPY)" }
    ];
  }

  public async RequestPasswordChange(body: RequestPasswordChangeDto): Promise<ApiResult> {
    return this.http.Post("User/resetPassword", body);
  }

  public async ChangePassword(body: ChangePasswordDto): Promise<ApiResult> {
    return this.http.Post("User/changePassword", body);
  }

  public async UpdateLanguage(selectedLang: string): Promise<ApiResult> {
    const result = await this.http.Post("User/language", null, { params: new HttpParams().set("lang", selectedLang) });

    if (result.success)
      this.selectedLang = selectedLang;
    
    /* if (result.success) {
      this.languages.forEach((x, i) => {
        x.IsSelected = x.Id == selectedLang;
      });
    } */

    return result;
  }

  public async UpdateCurrency(selectedCurrency: string): Promise<ApiResult> {
    const result = await this.http.Post("User/currency", null, { params: new HttpParams().set("currency", selectedCurrency) });

    if (result.success)
      this.selectedCurrency = selectedCurrency;
    
    /* if (result.success) {
      this.languages.forEach((x, i) => {
        x.IsSelected = x.Id == selectedCurrency;
      });
    } */

    return result;
  }
}
