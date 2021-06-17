import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { UserService } from 'src/app/user/services/user.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  langChanging = false;
  langChangeSuccess = false;
  langChangeError = false;
  currencyChanging = false;
  currencyChangeSuccess = false;
  currencyChangeError = false;
  
  constructor(public userService: UserService, private authService: AuthService) {
    
  }

  ngOnInit(): void {
  }

  onLanguageChange($event: any): void {
    const lang = $event.target.value;

    this.langChanging = true;
    this.langChangeSuccess = false;
    this.langChangeError = false;

    this.userService.UpdateLanguage(lang).then((x) => {
      this.langChanging = false;
      this.langChangeSuccess = x.success;
      this.langChangeError = !x.success;
    });
  }
  
  onCurrencyChange($event: any): void {
    this.currencyChanging = true;
    this.currencyChangeSuccess = false;
    this.currencyChangeError = false;

    this.userService.UpdateCurrency($event.target.value).then((x) => {
      this.currencyChanging = false;
      this.currencyChangeSuccess = x.success;
      this.currencyChangeError = !x.success;
    });
  }
}
