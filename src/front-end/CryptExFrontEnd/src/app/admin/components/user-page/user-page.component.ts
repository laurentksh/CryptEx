import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AlertType, SnackBarCreate } from 'src/app/components/snackbar/snack-bar';
import { BankAccountStatus } from 'src/app/deposit-withdraw/models/bank-account-view-model';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AccountStatus, FullUserViewModel } from '../../models/full-user-view-model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.scss']
})
export class UserPageComponent implements OnInit {
  basePath = "User.MyAccount.";
  baseAdminPath = "Admin.UserPage."
  userId: string;
  user: FullUserViewModel;
  ibanStatusRef = BankAccountStatus;
  AccountStatusRef = AccountStatus;

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

    this.userId = id;
    this.loadUser();
  }

  loadUser(): void {
    this.admin.GetFullUser(this.userId).then(x => {
      if (x.success) {
        this.user = x.content;
      } else {
        this.router.navigate(["admin"]);
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load the user.", AlertType.Error));
      }
    })
  }

  switchAccountState(): void {
    let status: AccountStatus;

    if (this.user.status == AccountStatus.active)
      status = AccountStatus.inactive;
    else
      status = AccountStatus.active;
    
    this.admin.SetAccountStatus(this.user.id, status).then(x => {
      if (x.success) {
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "User account status was successfully changed.", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "An error occured while changing the user's account status.", AlertType.Error));
      }

      this.loadUser();
    });
  }

  switchStateBankAccount(): void {
    let status : BankAccountStatus;

    if (this.user.bankAccount == null)
      return;

    if (this.user.bankAccount.status == BankAccountStatus.approved)
      status = BankAccountStatus.refused;
    else
      status = BankAccountStatus.approved;
    
    this.admin.SetBankAccountStatus(this.user.bankAccount.id, status).then(x => {
      if (x.success) {
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "User's bank account status was successfully changed.", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "An error occured while changing the user's bank account.", AlertType.Error));
      }

      this.loadUser();
    })
  }

  getBirthDayDate(): string {
    return new Date(Date.parse(this.user.birthDay)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric' });
  }

  getCreationDate(): string {
    return new Date(Date.parse(this.user.creationDate)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric' });
  }

  getIbanStatusColor(): string {
    switch (this.user.bankAccount.status) {
      case BankAccountStatus.approved:
        return "green-500";
      case BankAccountStatus.refused:
        return "red-500";
      case BankAccountStatus.notProcessed:
        return "gray-500";
      default:
        break;
    }
  }
}
