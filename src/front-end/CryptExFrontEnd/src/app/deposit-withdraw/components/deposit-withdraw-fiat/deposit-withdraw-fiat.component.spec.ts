import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepositWithdrawFiatComponent } from './deposit-withdraw-fiat.component';

describe('DepositWithdrawComponent', () => {
  let component: DepositWithdrawFiatComponent;
  let fixture: ComponentFixture<DepositWithdrawFiatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DepositWithdrawFiatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DepositWithdrawFiatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
