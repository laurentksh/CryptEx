import { Component } from '@angular/core';
import { SnackBar } from './components/snackbar/snack-bar';
import { AlertType } from './components/snackbar/snack-bar';
import { SnackbarService } from './services/snackbar.service';
import { UserService } from './user/services/user.service';
import { TranslateService } from '@ngx-translate/core'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'CryptEx';

  constructor(private userService: UserService, private translateService: TranslateService) {

  }

  ngOnInit(): void {
    if (!this.userService.IsLangSet) {
      const browserLang = this.translateService.getBrowserLang();

      const langIndex = this.userService.languages.findIndex((x) => { x.Id == browserLang});

      if (langIndex != -1) {
        const lang = this.userService.languages[langIndex];

        this.userService.UpdateLanguage(lang.Id);
      }
    } else {
      this.translateService.use(this.userService.SelectedLang);
    }
  }
}
