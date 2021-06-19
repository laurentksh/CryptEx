import { WalletPairViewModel } from "src/app/wallet/models/wallet-pair-view-model";

export interface AssetConversionLockViewModel {
    id: string;
    expirationUtc: string;
    pair: WalletPairViewModel;
}