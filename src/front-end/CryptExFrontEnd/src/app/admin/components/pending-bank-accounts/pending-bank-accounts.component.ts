import { Component, OnInit } from '@angular/core';
import { BankAccountViewModel } from 'src/app/deposit-withdraw/models/bank-account-view-model';

@Component({
  selector: 'app-pending-bank-accounts',
  templateUrl: './pending-bank-accounts.component.html',
  styleUrls: ['./pending-bank-accounts.component.scss']
})
export class PendingBankAccountsComponent implements OnInit {
  bankAccounts: BankAccountViewModel[];

  constructor() { }

  ngOnInit(): void {
  }

}
