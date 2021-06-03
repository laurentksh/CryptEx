import { Component, OnInit } from '@angular/core';
import { CryptoDepositViewModel } from '../../models/crypto-deposit-view-model';
import { DepositViewModel } from '../../models/deposit-view-model';
import { FiatDepositViewModel } from '../../models/fiat-deposit-view-model';
import { DepositWithdrawService } from '../../services/deposit-withdraw.service';

@Component({
  selector: 'app-deposit-withdraw-home',
  templateUrl: './deposit-withdraw-home.component.html',
  styleUrls: ['./deposit-withdraw-home.component.scss']
})
export class DepositWithdrawHomeComponent implements OnInit {
  constructor(public depWithService: DepositWithdrawService) { }

  ngOnInit(): void {
    this.depWithService.StartConnection();
    this.depWithService.Listen();
  }

}