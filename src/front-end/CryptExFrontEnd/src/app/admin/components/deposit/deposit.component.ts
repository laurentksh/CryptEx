import { Component, Input, OnInit } from '@angular/core';
import { DepositViewModel } from 'src/app/deposit-withdraw/models/deposit-view-model';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.scss']
})
export class DepositComponent implements OnInit {
  @Input() deposit: DepositViewModel;

  constructor() { }
  
  ngOnInit(): void {
  }

}
