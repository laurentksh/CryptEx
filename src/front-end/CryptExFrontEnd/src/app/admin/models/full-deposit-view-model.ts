import { PaymentStatus } from "src/app/deposit-withdraw/models/deposit-view-model";
import { UserViewModel } from "src/app/user/models/user-view-model";
import { WalletType, WalletViewModel } from "src/app/user/models/wallet-view-model";

export interface FullDepositViewModel {
    id: string;
    amount: number;
    status: PaymentStatus;
    walletType: WalletType;
    date: string;
    walletId: string;
    wallet: WalletViewModel;
    user: UserViewModel;
    userId: string;
}