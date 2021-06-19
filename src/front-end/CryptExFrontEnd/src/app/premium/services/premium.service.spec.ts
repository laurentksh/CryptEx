import { TestBed } from '@angular/core/testing';

import { PremiumService } from './premium.service';

describe('PremiumService', () => {
  let service: PremiumService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PremiumService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
