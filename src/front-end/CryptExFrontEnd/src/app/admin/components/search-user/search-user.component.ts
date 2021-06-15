import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserViewModel } from 'src/app/user/models/user-view-model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.scss']
})
export class SearchUserComponent implements OnInit {
  searchResults: UserViewModel[];
  searchInput: string;
  timer: NodeJS.Timeout;
  typing: boolean;

  constructor(private admin: AdminService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
  }

  onChange(): void {
    this.typing = true;
    clearTimeout(this.timer);
    this.timer = setTimeout(() => this.doSearch(), 1500);
  }

  async doSearch(): Promise<void> {
    this.typing = false;
    const result = await this.admin.SearchUser(this.searchInput);

    if (result.success)
      this.searchResults = result.content;
  }

  redirectTo(id: string): void {
    this.router.navigate(["../user", id], { relativeTo: this.activatedRoute });
  }
}
