import { Component, OnInit } from '@angular/core';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { UserViewModel } from 'src/app/user/models/user-view-model';
import { UserService } from 'src/app/user/services/user.service';
import { StatsViewModel } from '../../models/stats-view-model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  loading = true;
  stats: StatsViewModel;

  constructor(private userService: UserService, private adminService: AdminService, private snack: SnackbarService) { }

  ngOnInit(): void {
    this.userService.RefreshUser();
    this.adminService.GetStats().then(x => {
      this.loading = false;
      if (x.success)
        this.stats = x.content;
      else
        this.snack.ShowSnackbar(new SnackBarCreate("Could not load statistics", "An unknown error occured while loading statistics, please try again later.", AlertType.Error));
    });
  }

  getUser(): UserViewModel {
    return this.userService.User;
  }

}
