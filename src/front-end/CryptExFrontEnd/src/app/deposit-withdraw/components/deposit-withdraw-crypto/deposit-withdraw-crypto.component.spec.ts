import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepositWithdrawCryptoComponent } from './deposit-withdraw-crypto.component';

describe('DepositWithdrawCryptoComponent', () => {
  let component: DepositWithdrawCryptoComponent;
  let fixture: ComponentFixture<DepositWithdrawCryptoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DepositWithdrawCryptoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DepositWithdrawCryptoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
