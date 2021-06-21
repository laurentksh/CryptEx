import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { CryptoDepositViewModel } from '../models/crypto-deposit-view-model';
import { DepositViewModel } from '../models/deposit-view-model';
import { FiatDepositViewModel } from '../models/fiat-deposit-view-model';
import * as signalR from "@microsoft/signalr";
import { EnvironmentService } from 'src/environments/service/environment.service';
import { AuthService } from 'src/app/auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DepositWithdrawService {
  public deposits: DepositViewModel[];

  private hubConnection: signalR.HubConnection;

  public async BeginSignalR(): Promise<ApiResult> {
    const update = await this.GetDeposits();
    this.deposits = update.content;

    this.buildConnection();
    await this.hubConnection.start();
    this.listen();

    return { success: update.success, error: update.error } as ApiResult;
  }

  public async EndSignalR(): Promise<void> {
    await this.hubConnection.stop();
  }

  private buildConnection(): void {
    const options: signalR.IHttpConnectionOptions = {
      transport: signalR.HttpTransportType.LongPolling,
      accessTokenFactory: () => {
        if (this.auth.IsAuthenticated)
          return this.auth.JWToken;
        else
          return null
      }
    };

    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(this.env.production ? signalR.LogLevel.Warning : signalR.LogLevel.Trace)
      .withAutomaticReconnect()
      .withUrl(this.env.apiBaseUrl + "feed/deposits", options)
      .build();
  }

  private listen(): void {
    this.hubConnection?.on('depositsdata', (data) => {
      console.log("data received", data);
      this.refreshData(data);
    })
  }

  constructor(private http: CustomHttpClientService, private env: EnvironmentService, private auth: AuthService) { }

  private refreshData(data: DepositViewModel[]): void {
    data.forEach(newElement => {
      var index = this.deposits.findIndex(x => x.id == element.id)

      if (index != -1) {
        var element = this.deposits[index];
        element.amount = newElement.amount;
        element.status = newElement.status;
        element.wallet = newElement.wallet;
        element.walletId = newElement.walletId;
      } else {
        this.deposits.push(newElement);
      }
    });
  }

  public async GetDeposits(): Promise<ApiResult<DepositViewModel[]>> {
    return this.http.Get<DepositViewModel[]>("Payment/deposits");
  }

  public async RefreshDeposits(): Promise<ApiResult> {
    return this.http.Get("Payment/deposits", { params: new HttpParams().set("signalR", "true") });
  }

  public async DepositFiat(amount: number): Promise<ApiResult<FiatDepositViewModel>> {
    return this.http.Post<FiatDepositViewModel>("Payment/deposit/fiat", null, { params: new HttpParams().set("amount", amount.toString()) });
  }

  public async DepositCrypto(walletId: string): Promise<ApiResult<CryptoDepositViewModel>> {
    return this.http.Post<CryptoDepositViewModel>("Payment/deposit/crypto", null, { params: new HttpParams().set("walletId", walletId) });
  }

  public async WithdrawFiat(amount: number): Promise<ApiResult<CryptoDepositViewModel>> {
    return this.http.Post<CryptoDepositViewModel>("Payment/withdraw", null, { params: new HttpParams().set("amount", amount.toString()) });
  }
}