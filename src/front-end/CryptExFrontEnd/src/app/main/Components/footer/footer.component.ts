import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
  
  constructor(public userService: UserService) { //Moves these constants in a service.
    
  }

  ngOnInit(): void {
  }

  onLanguageChange($event): void {
    this.langChanging = true;
    this.langChangeSuccess = false;
    this.langChangeError = false
    this.userService.UpdateLanguage($event.target.value).then((x) => {
      this.langChanging = false;
      this.langChangeSuccess = x.success;
      this.langChangeError = !x.success;

      debugger;
    });
  }
  
  onCurrencyChange($event): void {
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
