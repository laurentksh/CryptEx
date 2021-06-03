import { DepositViewModel } from "./deposit-view-model";

export interface CryptoDepositViewModel extends DepositViewModel {
    WalletAddress: string;
}
