import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { BankAccountViewModel } from 'src/app/deposit-withdraw/models/bank-account-view-model';
import { DepositViewModel, PaymentStatus } from 'src/app/deposit-withdraw/models/deposit-view-model';
import { UserViewModel } from 'src/app/user/models/user-view-model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: CustomHttpClientService) { }

  public async GetUser(id: string): Promise<ApiResult<UserViewModel>> {
    return this.http.Get("Admin/user", { params: new HttpParams().set("userId", id) });
  }

  public async GetDeposits(): Promise<ApiResult<DepositViewModel[]>> {
    return this.http.Get("Admin/deposits");
  }

  public async SetPaymentStatus(sessionId: string, status: PaymentStatus): Promise<ApiResult> {
    return this.http.Post("Admin/setPaymentStatus", null, { params: new HttpParams().set("sessionId", sessionId).set("status", status.toString()) })
  }

  public async GetPendingBankAccounts(): Promise<ApiResult<BankAccountViewModel>> {
    return this.http.Get("Admin/pendingBankAccounts");
  }
}
