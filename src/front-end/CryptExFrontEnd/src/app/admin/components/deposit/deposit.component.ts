import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FullDepositViewModel } from '../../models/full-deposit-view-model';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.scss']
})
export class DepositComponent implements OnInit {
  @Input() deposit: FullDepositViewModel;
  @Output() redirectTo = new EventEmitter<string>();
  
  constructor() { }
  
  ngOnInit(): void {
  }

  redirectToUserPage(): void {
    this.redirectTo.emit(this.deposit.userId);
  }

  getDate(): string {
    return new Date(Date.parse(this.deposit.date)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' });
  }
}