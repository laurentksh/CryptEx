import { PaymentStatus } from "src/app/deposit-withdraw/models/deposit-view-model";
import { WalletPairViewModel } from "src/app/wallet/models/wallet-pair-view-model";

export interface AssetConverssionViewModel {
    id: string;
    amount: number;
    status: PaymentStatus;
    pair: WalletPairViewModel;
}