import { WalletViewModel } from "src/app/user/models/wallet-view-model";
import { BankAccountViewModel } from "./bank-account-view-model";
import { PaymentStatus } from "./deposit-view-model";

export interface FiatWithdrawalViewModel {
    id: string;

    amount: number;

    status: PaymentStatus;

    date: string;

    bankAccount: BankAccountViewModel;

    wallet: WalletViewModel;
}
