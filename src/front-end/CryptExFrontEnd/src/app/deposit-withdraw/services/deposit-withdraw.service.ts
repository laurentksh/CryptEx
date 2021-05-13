import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { CryptoDepositViewModel } from '../models/crypto-deposit-view-model';
import { FiatDepositViewModel } from '../models/fiat-deposit-view-model';

@Injectable({
  providedIn: 'root'
})
export class DepositWithdrawService {
  
  constructor(private http: CustomHttpClientService) { }

  public async DepositFiat(amount: number): Promise<ApiResult<FiatDepositViewModel>> {
    return this.http.Post<FiatDepositViewModel>("Payment/deposit/fiat", null, { params: new HttpParams().set("amount", amount.toString()) });
  }

  public async DepositCrypto(walletId: string): Promise<ApiResult<CryptoDepositViewModel>> {
    return this.http.Post<CryptoDepositViewModel>("Payment/deposit/crypto", null, { params: new HttpParams().set("walletId", walletId) });
  }
}