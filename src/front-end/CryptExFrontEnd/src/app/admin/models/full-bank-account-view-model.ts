import { BankAccountStatus } from "src/app/deposit-withdraw/models/bank-account-view-model";
import { UserViewModel } from "src/app/user/models/user-view-model";

export interface FullBankAccountViewModel {
    id: string;

    iban: string;

    creationDate: string;

    decisionDate: string;

    status: BankAccountStatus;

    user: UserViewModel;

    userId: string;
}