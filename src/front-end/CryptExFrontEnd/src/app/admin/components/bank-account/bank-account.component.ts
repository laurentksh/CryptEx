import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FullBankAccountViewModel } from '../../models/full-bank-account-view-model';

@Component({
  selector: 'app-bank-account',
  templateUrl: './bank-account.component.html',
  styleUrls: ['./bank-account.component.scss']
})
export class BankAccountComponent implements OnInit {
  @Input() bankAccount: FullBankAccountViewModel;
  @Output() redirectTo = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
  }

  redirectToUserPage(): void {
    this.redirectTo.emit(this.bankAccount.userId);
  }

  getCreationDate(): string {
    return new Date(Date.parse(this.bankAccount.creationDate)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' });
  }
}
