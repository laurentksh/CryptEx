import { Component, OnInit } from '@angular/core';
import { DepositViewModel } from 'src/app/deposit-withdraw/models/deposit-view-model';

@Component({
  selector: 'app-deposits',
  templateUrl: './deposits.component.html',
  styleUrls: ['./deposits.component.scss']
})
export class DepositsComponent implements OnInit {
  deposits: DepositViewModel[];

  constructor() { }

  ngOnInit(): void {
  }

}
