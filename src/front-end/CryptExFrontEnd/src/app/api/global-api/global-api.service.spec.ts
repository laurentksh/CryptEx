import { TestBed } from '@angular/core/testing';

import { GlobalApiService } from './global-api.service';

describe('ApiService', () => {
  let service: GlobalApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlobalApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
