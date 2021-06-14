export interface IbanViewModel{
  iban: string;
  status: BankAccountStatus;
}

export enum BankAccountStatus {
  notProcessed = -1,
  approved = 1,
  refused = 0
}
