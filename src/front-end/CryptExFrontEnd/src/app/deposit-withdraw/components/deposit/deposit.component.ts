import { Component, Input, OnInit } from '@angular/core';
import { DepositViewModel, PaymentStatus } from '../../models/deposit-view-model';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.scss']
})
export class DepositComponent implements OnInit {
  @Input() public deposit: DepositViewModel;

  public readonly PaymentStatusRef = PaymentStatus;

  constructor() { }

  ngOnInit(): void {
  }

  getDate(): Date {
    return new Date(Date.parse(this.deposit.date));
  }
}
