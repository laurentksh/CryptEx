import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { FullUserViewModel } from '../../models/full-user-view-model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.scss']
})
export class UserPageComponent implements OnInit {
  user: FullUserViewModel;

  constructor(private route: ActivatedRoute, private router: Router, private admin: AdminService, private snack: SnackbarService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.handleParams(params);
    })
  }

  handleParams(params: Params): void {
    const id = params["id"];

    if (id == null) {
      this.router.navigate(["notfound"]);
      this.snack.ShowSnackbar(new SnackBarCreate("User not found", "No user with the specified id was found.", AlertType.Error));
      return;
    }

    this.admin.GetFullUser(id).then(x => {
      if (x.success) {
        this.user = x.content;
      } else {
        this.router.navigate(["admin"]);
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load the user.", AlertType.Error));
      }
    })
  }
}
