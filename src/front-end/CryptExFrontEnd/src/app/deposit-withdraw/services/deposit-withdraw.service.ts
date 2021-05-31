import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { CryptoDepositViewModel } from '../models/crypto-deposit-view-model';
import { DepositViewModel } from '../models/deposit-view-model';
import { FiatDepositViewModel } from '../models/fiat-deposit-view-model';
import * as signalR from "@microsoft/signalr";
import { EnvironmentService } from 'src/environments/service/environment.service';

@Injectable({
  providedIn: 'root'
})
export class DepositWithdrawService {
  public deposits: DepositViewModel[];

  private hubConnection: signalR.HubConnection;

  public StartConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.env.apiBaseUrl + "live/deposits")
      .build();
  }

  public Listen(): void {
    this.hubConnection?.on('depositsUpdate', (data) => {
      this.deposits = data;
    })
  }

  constructor(private http: CustomHttpClientService, private env: EnvironmentService) { }

  public async DepositFiat(amount: number): Promise<ApiResult<FiatDepositViewModel>> {
    return this.http.Post<FiatDepositViewModel>("Payment/deposit/fiat", null, { params: new HttpParams().set("amount", amount.toString()) });
  }

  public async DepositCrypto(walletId: string): Promise<ApiResult<CryptoDepositViewModel>> {
    return this.http.Post<CryptoDepositViewModel>("Payment/deposit/crypto", null, { params: new HttpParams().set("walletId", walletId) });
  }

  public async WithdrawFiat(amount: number): Promise<ApiResult<CryptoDepositViewModel>> {
    return this.http.Post<CryptoDepositViewModel>("Payment/withdraw", null);
  }
}