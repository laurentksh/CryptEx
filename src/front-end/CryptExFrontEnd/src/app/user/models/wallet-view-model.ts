import { WalletPairViewModel } from "./wallet-pair-view-model";

export interface WalletViewModel {
    id: string;
    ticker: string;
    fullName: string;
    type: WalletType;
    selectedCurrencyPair: WalletPairViewModel;
}

export enum WalletType {
    Fiat = 1,
    Crypto = 2
}
