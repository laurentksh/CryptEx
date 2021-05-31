import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepositWithdrawHomeComponent } from './deposit-withdraw-home.component';

describe('DepositTypeChoiceComponent', () => {
  let component: DepositWithdrawHomeComponent;
  let fixture: ComponentFixture<DepositWithdrawHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DepositWithdrawHomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DepositWithdrawHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
