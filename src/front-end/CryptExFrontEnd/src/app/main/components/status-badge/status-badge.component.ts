import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'status-badge',
  templateUrl: './status-badge.component.html',
  styleUrls: ['./status-badge.component.scss']
})
export class StatusBadgeComponent implements OnInit {
  @Input() text: string;
  @Input() color: string = null;

  constructor() { }

  ngOnInit(): void {
  }

}
