import { WalletViewModel } from "src/app/user/models/wallet-view-model";

export interface DepositViewModel {
    Id: string;
    Amount: number;
    WalletId: string;
    Wallet: WalletViewModel;
}
