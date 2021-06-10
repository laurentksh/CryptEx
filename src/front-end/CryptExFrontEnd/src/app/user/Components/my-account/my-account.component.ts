import {Component, OnInit} from '@angular/core';
import {AddressViewModel} from "../../models/address-view-model";
import {IbanViewModel} from "../../models/iban-view-model";
import {Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {UserUpdateDto} from "../../models/user-update-dto";
import {SnackbarService} from "../../../services/snackbar.service";
import {AlertType, SnackBarCreate} from "../../../components/snackbar/snack-bar";
import {UserViewModel} from "../../models/user-view-model";
import {AddressDto} from "../../models/address-dto";
import {IbanDto} from "../../models/iban-dto";

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  addressVm: AddressViewModel;
  userUpdateDto: UserUpdateDto;
  userVm : UserViewModel;
  adresseDto: AddressDto;
  iban: IbanViewModel;
  ibanDto: IbanDto;

  clickedUser: boolean = false;
  clickedPhone: boolean = false;
  clickedEmail: boolean = false;
  clickedAddress: boolean = false;
  clickedCountry: boolean = false;
  clickedIban: boolean = false;

  constructor(private router: Router, private userService: UserService, private snack: SnackbarService) {
    this.addressVm = {} as AddressViewModel;
    this.userUpdateDto = {} as UserUpdateDto;
    this.iban = {} as IbanViewModel;
    this.userVm = {} as UserViewModel;
    this.adresseDto = {}  as AddressDto;
    this.ibanDto = {} as IbanDto;
  }

  ngOnInit(): void {
    this.userService.RefreshUser().then(x => {
      this.userVm = this.userService.User;
    })
  }

  updateUser(): void {
    this.userService.UpdateUser(this.userUpdateDto).then(x =>
    {
      if (!x.success)
      {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Update user error : " + x.error.message, AlertType.Error));
      } else {
        this.snack.ShowSnackbar(new SnackBarCreate("Error", "Update succeed", AlertType.Success));
      }
    })
  }

  doClick(): void{
    this.clickedUser = !this.clickedUser;
    this.clickedPhone = !this.clickedPhone;
    this.clickedCountry = !this.clickedCountry;
    this.clickedEmail = !this.clickedEmail;
    this.clickedAddress = !this.clickedAddress;
    this.clickedIban = !this.clickedIban;

    this.updateUser();
  }


}
