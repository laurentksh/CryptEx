import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { BankAccountViewModel } from 'src/app/deposit-withdraw/models/bank-account-view-model';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-pending-bank-accounts',
  templateUrl: './pending-bank-accounts.component.html',
  styleUrls: ['./pending-bank-accounts.component.scss']
})
export class PendingBankAccountsComponent implements OnInit {
  bankAccounts: BankAccountViewModel[];

  constructor(private adminService: AdminService, private snack: SnackbarService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.adminService.GetPendingBankAccounts().then(x => {
      if (x.success) {
        this.bankAccounts = x.content;
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load pending bank accounts.", AlertType.Error));
      }
    })
  }

  redirectTo(id: string): void {
    this.router.navigate(["../user", id], { relativeTo: this.activatedRoute });
  }
}
