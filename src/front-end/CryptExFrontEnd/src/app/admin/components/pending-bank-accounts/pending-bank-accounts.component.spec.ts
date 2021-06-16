import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingBankAccountsComponent } from './pending-bank-accounts.component';

describe('PendingBankAccountsComponent', () => {
  let component: PendingBankAccountsComponent;
  let fixture: ComponentFixture<PendingBankAccountsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PendingBankAccountsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingBankAccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
