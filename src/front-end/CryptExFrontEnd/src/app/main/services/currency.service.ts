import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  private onCurrencyChangeSource = new Subject<string>();

  /**
   * Notify listeners of currency change.
   */
  public OnCurrencyChange = this.onCurrencyChangeSource.asObservable();

  constructor() { }

  /**
   * INTERNAL METHOD, DO NOT USE.
   * @param currency The new currency
   */
  public changeCurrencyInternal(currency: string): void {
    this.onCurrencyChangeSource.next(currency);
  }
}
