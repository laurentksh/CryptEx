import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-percentage-card',
  templateUrl: './percentage-card.component.html',
  styleUrls: ['./percentage-card.component.scss']
})
export class PercentageCardComponent implements OnInit {
  @Input() oldValue: number;
  @Input() newValue: number;
  percentage: number;

  constructor() { }

  ngOnInit(): void {
    if (this.oldValue == null || this.newValue == null)
      return;
      
    this.percentage = (this.oldValue / this.newValue) * 100
  }

}
