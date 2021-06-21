import { BankAccountViewModel } from "src/app/deposit-withdraw/models/bank-account-view-model";
import { AddressViewModel } from "src/app/user/models/address-view-model";

export interface FullUserViewModel {
    id: string;

    firstName: string;

    lastName: string;

    birthDay: string;

    status: AccountStatus;

    email: string;
    
    phoneNumber: string;

    preferedLanguage: string;

    preferedCurrency: string;

    creationDate: string;

    bankAccount: BankAccountViewModel;

    address: AddressViewModel;
}

export enum AccountStatus {
    active = 1,
    inactive = -1
}