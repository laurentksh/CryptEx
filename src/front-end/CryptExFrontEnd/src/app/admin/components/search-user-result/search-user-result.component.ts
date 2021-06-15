import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { UserViewModel } from 'src/app/user/models/user-view-model';

@Component({
  selector: 'app-search-user-result',
  templateUrl: './search-user-result.component.html',
  styleUrls: ['./search-user-result.component.scss']
})
export class SearchUserResultComponent implements OnInit {
  @Input() user: UserViewModel;
  @Output() redirectTo = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void {
  }

  redirectToUserPage(): void {
    this.redirectTo.emit(this.user.id);
  }
}