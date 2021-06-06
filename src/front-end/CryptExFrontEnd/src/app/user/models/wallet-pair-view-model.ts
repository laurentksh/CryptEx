import { WalletViewModel } from "./wallet-view-model";

export interface WalletPairViewModel {
        /**
         * Left side of the pair (e.g: BTC)
         */
        left: WalletViewModel;

        /**
         * Right side of the pair (e.g: USD)
         */
        right: WalletViewModel;
        
        /**
         * Rate for which the left is traded for the right (1:{rate})
         * Example for BTCUSD (as of 17.05.2021): 45403.06 
         */
        rate: number;
}
