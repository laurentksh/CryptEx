import { TestBed } from '@angular/core/testing';

import { AssetConvertService } from './asset-convert.service';

describe('AssetConvertService', () => {
  let service: AssetConvertService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssetConvertService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
