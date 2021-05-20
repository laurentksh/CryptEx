import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepositWithdrawComponent } from './deposit-withdraw.component';

describe('DepositWithdrawComponent', () => {
  let component: DepositWithdrawComponent;
  let fixture: ComponentFixture<DepositWithdrawComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DepositWithdrawComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DepositWithdrawComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
