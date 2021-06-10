import { DepositViewModel } from "./deposit-view-model";

export interface FiatDepositViewModel extends DepositViewModel {
    sessionId: string;
}
