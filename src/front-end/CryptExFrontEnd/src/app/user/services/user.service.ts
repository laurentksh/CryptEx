import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { AuthService } from 'src/app/auth/services/auth.service';
import { ChangePasswordDto } from '../models/change-password-dto';
import { Currency } from '../models/currency';
import { Language } from '../models/language';
import { RequestPasswordChangeDto } from '../models/request-password-change-dto';
import { UserViewModel } from '../models/user-view-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public languages: Language[];
  public currencies: Currency[];

  constructor(private http: CustomHttpClientService, private authService: AuthService, private translateService: TranslateService) {
    this.languages = [
      { Id: "en-us", DisplayText: "English" },
      { Id: "fr-fr", DisplayText: "Français" },
      { Id: "de-de", DisplayText: "Deutsch" }
    ];

    this.currencies = [
      { Id: "USD" },
      { Id: "CHF" },
      { Id: "EUR" },
      { Id: "GBP" },
      { Id: "CAD" },
      { Id: "AUD" },
      { Id: "JPY" }
    ];
  }

  public get User(): UserViewModel {
    return JSON.parse(localStorage.getItem("user")) as UserViewModel;
  }

  public get IsLangSet(): boolean {
      return localStorage.getItem("lang") != null;
  }

  public get IsCurrencySet(): boolean {
      return localStorage.getItem("currency") != null;
  }

  public get SelectedLang() {
    return localStorage.getItem("lang") ?? "en-us";
  }

  public get SelectedCurrency() {
    return localStorage.getItem("currency") ?? "USD";
  }

  private setSelectedLang(val: string) {
    localStorage.setItem("lang", val);
  }

  private setSelectedCurrency(val: string) {
    localStorage.setItem("currency", val);
  }

  public async RefreshUser(): Promise<ApiResult<UserViewModel>> {
    const user = await this.http.Get<UserViewModel>("User");
    localStorage.setItem("user", JSON.stringify(user.content));

    this.setSelectedLang(user.content.preferedLanguage);
    this.setSelectedCurrency(user.content.preferedCurrency);

    this.translateService.use(user.content.preferedLanguage);

    return user;
  }

  public async RequestPasswordChange(body: RequestPasswordChangeDto): Promise<ApiResult> {
    return this.http.Post("User/resetPassword", body);
  }

  public async ChangePassword(body: ChangePasswordDto): Promise<ApiResult> {
    return this.http.Post("User/changePassword", body);
  }

  public async UpdateLanguage(selectedLang: string): Promise<ApiResult> {
    var result = null;
    if (this.authService.IsAuthenticated) {
      result = await this.http.Post("User/language", null, { params: new HttpParams().set("lang", selectedLang) });

      await this.RefreshUser();
    } else {
      result = { success: true } as ApiResult;
    }

    if (result.success) {
        this.setSelectedLang(selectedLang);
        this.translateService.use(selectedLang);
    }
    
    return result;
  }

  public async UpdateCurrency(selectedCurrency: string): Promise<ApiResult> {
    if (this.authService.IsAuthenticated) {
      const result = await this.http.Post("User/currency", null, { params: new HttpParams().set("currency", selectedCurrency) });

      if (result.success)
        this.setSelectedCurrency(selectedCurrency);
        
      await this.RefreshUser();
      
      return result;
    } else {
      this.setSelectedCurrency(selectedCurrency);

      return { success: true } as ApiResult;
    }
  }
}
