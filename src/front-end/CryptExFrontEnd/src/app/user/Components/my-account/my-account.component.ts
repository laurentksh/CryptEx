import { Component, OnInit } from '@angular/core';
import { AddressViewModel } from "../../models/address-view-model";
import { Router } from "@angular/router";
import { UserService } from "../../services/user.service";
import { UserUpdateDto } from "../../models/user-update-dto";
import { SnackbarService } from "../../../services/snackbar.service";
import { AlertType, SnackBarCreate } from "../../../components/snackbar/snack-bar";
import { UserViewModel } from "../../models/user-view-model";
import { AddressDto } from "../../models/address-dto";
import { IbanDto } from "../../models/iban-dto";
import { CountryViewModel } from 'src/app/api/models/country-view-model';
import { GlobalApiService } from 'src/app/api/global-api/global-api.service';
import { BankAccountStatus, BankAccountViewModel } from 'src/app/deposit-withdraw/models/bank-account-view-model';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  basePath = "User.MyAccount.";

  countries: CountryViewModel[] = null;

  loading = true;
  hidePage = false;

  userVm: UserViewModel;
  userUpdateDto: UserUpdateDto = {} as UserUpdateDto;
  addressVm: AddressViewModel;
  addressDto: AddressDto = {} as AddressDto;
  ibanVm: BankAccountViewModel;
  ibanDto: IbanDto = {} as IbanDto;

  clickedName: boolean = false;
  clickedPhone: boolean = false;
  clickedEmail: boolean = false;
  clickedBirthDay: boolean = false;
  clickedAddress: boolean = false;
  clickedIban: boolean = false;

  constructor(private router: Router, private userService: UserService, private globalService: GlobalApiService, private snack: SnackbarService) { }

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    var loadError = false;

    var user = await this.userService.RefreshUser();

    if (user.success)
      this.userVm = user.content;
    else
      loadError = true;

    if (loadError) {
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user data.", AlertType.Error));
      this.hidePage = true;
      this.loading = false;
      return;
    }

    var address = await this.userService.GetAddress();

    if (address.success && address.content != null)
      this.addressVm = address.content;
    if (!address.success)
      loadError = true;

    if (loadError) {
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user data.", AlertType.Error));
      this.hidePage = true;
      this.loading = false;
      return;
    }

    var countries = await this.globalService.GetCountries();

    if (countries.success) {
      this.countries = countries.content;
    } else {
      loadError = true;
    }

    if (loadError) {
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user data.", AlertType.Error));
      this.hidePage = true;
      this.loading = false;
      return;
    }

    var iban = await this.userService.GetIban();

    if (iban.success && iban.content != null)
      this.ibanVm = iban.content;
    if (!iban.success)
      loadError = true;

    if (loadError) {
      this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not load user data.", AlertType.Error));
      this.hidePage = true;
      this.loading = false;
      return;
    }

    this.loading = false;
  }

  updateUser(): void {
    this.userService.UpdateUser(this.userUpdateDto).then(x => {
      this.userUpdateDto = {} as UserUpdateDto;
      if (x.success) {
        this.userVm = x.content;
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "Your infos were successfully updated.", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not update basic infos.", AlertType.Error));
      }
    });
  }

  updateAddress(): void {
    this.userService.UpdateAddress(this.addressDto).then(x => {
      this.addressDto = {} as AddressDto;
      if (x.success) {
        this.addressVm = x.content;
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "Your address was successfully updated.", AlertType.Success));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not update address.", AlertType.Error));
      }
    });
  }

  updateIban(): void {
    this.userService.UpdateIban(this.ibanDto).then(x => {
      this.ibanDto = {} as IbanDto;
      if (x.success) {
        this.ibanVm = x.content;
        this.snack.ShowSnackbar(new SnackBarCreate("Success", "Your bank account is pending validation and will be approved in 1-5 business days.", AlertType.Success));
      } else {
        console.log(x.error);

        if (x.error.error.errors.Iban.length != 0)
          this.snack.ShowSnackbar(new SnackBarCreate("Error", x.error.error.errors.Iban[0], AlertType.Error));
        else
          this.snack.ShowSnackbar(new SnackBarCreate("Error", "Could not update bank account.", AlertType.Error));
      }
    });
  }

  doUpdateName(): void {
    if (this.clickedName)
      this.updateUser();

    this.clickedName = !this.clickedName;
  }

  doUpdatePhone(): void {
    if (this.clickedPhone)
      this.updateUser();

    this.clickedPhone = !this.clickedPhone;
  }

  doUpdateEmail(): void {
    if (this.clickedEmail)
      this.updateUser();

    this.clickedEmail = !this.clickedEmail;
  }

  doUpdateBirthDay(): void {
    if (this.clickedBirthDay)
      this.updateUser();

    this.clickedBirthDay = !this.clickedBirthDay;
  }

  doAddress(): void {
    if (this.clickedAddress)
      this.updateAddress();

    this.clickedAddress = !this.clickedAddress;
  }

  doIban(): void {
    if (this.clickedIban)
      this.updateIban();

    this.clickedIban = !this.clickedIban;
  }

  onCountryChange($event: any): void {
    this.addressDto.countryId = $event.target.value;
  }

  getBirthDayDate(): string {
    return new Date(Date.parse(this.userVm.birthDay)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric' });
  }

  getCreationDate(): string {
    return new Date(Date.parse(this.userVm.creationDate)).toLocaleString([], { day: 'numeric', month: 'long', year: 'numeric' });
  }

  getIbanStatusColor(): string {
    switch (this.ibanVm.status) {
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
