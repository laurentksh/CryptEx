import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  languages: string[];
  currencies: string[];

  constructor() { //Moves these constants in a service.
    this.languages = [
      "English",
      "French",
      "German"
    ];

    this.currencies = [
      "USD",
      "CHF",
      "EUR",
      "GBP",
      "CAD",
      "AUD",
      "JPY"
    ];
  }

  ngOnInit(): void {
  }

  onCurrencyChange($event): void {
    
  }

  onLanguageChange($event): void {

  }
}
