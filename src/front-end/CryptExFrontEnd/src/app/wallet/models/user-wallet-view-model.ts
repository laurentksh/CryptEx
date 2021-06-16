import { WalletViewModel } from "./wallet-view-model";

export interface UserWalletViewModel extends WalletViewModel {
    amount: number;
}