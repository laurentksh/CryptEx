import { WalletViewModel } from "src/app/wallet/models/wallet-view-model";

export interface DepositViewModel {
    id: string;
    amount: number;
    date: string;
    status: PaymentStatus;
    walletId: string;
    wallet: WalletViewModel;
}

export enum PaymentStatus {
    /**
     * The transaction has not been processed yet.
     */
    notProcessed = -1,

    /**
     * The transaction failed or was canceled.
     */
    failed = 0,

    /**
     * The transaction was successful.
     */
    success = 1,

    /**
     * The transaction is pending. (Most commonly this would indicate that the transaction was made using a payment method that takes time to process, like a bank wire)
     */
    pending = 2
}