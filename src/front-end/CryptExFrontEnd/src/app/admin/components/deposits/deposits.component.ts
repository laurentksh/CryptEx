import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { FullDepositViewModel } from '../../models/full-deposit-view-model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-deposits',
  templateUrl: './deposits.component.html',
  styleUrls: ['./deposits.component.scss']
})
export class DepositsComponent implements OnInit {
  deposits: FullDepositViewModel[];

  constructor(private adminService: AdminService, private snack: SnackbarService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.adminService.GetDeposits().then(x => {
      if (x.success) {
        this.deposits = x.content;
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load deposits.", AlertType.Error));
      }
    })
  }

  redirectTo(id: string): void {
    this.router.navigate(["../user", id], { relativeTo: this.activatedRoute });
  }
}
