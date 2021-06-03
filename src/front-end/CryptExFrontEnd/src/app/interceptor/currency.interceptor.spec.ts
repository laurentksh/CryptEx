import { TestBed } from '@angular/core/testing';

import { CurrencyInterceptor } from './currency.interceptor';

describe('CurrencyInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      CurrencyInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: CurrencyInterceptor = TestBed.inject(CurrencyInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
