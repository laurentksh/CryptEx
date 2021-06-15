export interface BankAccountViewModel {
    id: string;

    iban: string;

    creationDate: string;

    decisionDate: string;

    status: BankAccountStatus;
}

export enum BankAccountStatus {
    notProcessed = -1,
    approved = 1,
    refused = 0
}