import { TestBed } from '@angular/core/testing';

import { CustomHttpClientService } from './custom-http-client.service';

describe('CustomHttpClientService', () => {
  let service: CustomHttpClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomHttpClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
