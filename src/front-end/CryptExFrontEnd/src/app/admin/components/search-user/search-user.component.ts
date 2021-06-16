import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
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

  constructor(private admin: AdminService, private router: Router, private activatedRoute: ActivatedRoute, private snack: SnackbarService) { }

  ngOnInit(): void {
  }

  onChange(): void {
    this.typing = true;
    clearTimeout(this.timer);
    this.timer = setTimeout(() => this.doSearch(), 1500);
  }

  async doSearch(): Promise<void> {
    const result = await this.admin.SearchUser(this.searchInput);
    this.typing = false;
    
    if (result.success)
      this.searchResults = result.content;
    else
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load search results.", AlertType.Error));
  }

  redirectTo(id: string): void {
    this.router.navigate(["../user", id], { relativeTo: this.activatedRoute });
  }
}
