import { TotalViewModel } from "./total-view-model";

export interface TotalsViewModel {
    accountTotal: TotalViewModel;
    fiatTotal: TotalViewModel;
    cryptoTotal: TotalViewModel;

    accountTotalYtd: TotalViewModel;
    fiatTotalYtd: TotalViewModel;
    cryptoTotalYtd: TotalViewModel;

    performanceYtd: TotalViewModel;
}