export interface BankAccountViewModel {
    id: string;

    iban: string;

    status: BankAccountStatus;
}

export enum BankAccountStatus {
    NotProcessed = -1,
    Approved = 1,
    Refused = 0
}