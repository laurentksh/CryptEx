import { Component, Input, OnInit } from '@angular/core';
import { BankAccountViewModel } from 'src/app/deposit-withdraw/models/bank-account-view-model';

@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss']
})
export class BankAccountComponent implements OnInit {
  @Input() bankAccount: BankAccountViewModel;
  
  constructor() { }

  ngOnInit(): void {
  }

}
