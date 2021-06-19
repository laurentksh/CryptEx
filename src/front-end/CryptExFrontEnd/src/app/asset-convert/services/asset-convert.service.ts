import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { AssetConverssionDto } from '../models/asset-converssion-dto';
import * as signalR from "@microsoft/signalr";
import { AssetConverssionViewModel } from '../models/asset-converssion-view-model';
import { AuthService } from 'src/app/auth/services/auth.service';
import { EnvironmentService } from 'src/environments/service/environment.service';
import { HttpParams } from '@angular/common/http';
import { AssetConversionLockDto } from '../models/asset-conversion-lock-dto';
import { AssetConversionLockViewModel } from '../models/asset-conversion-lock-view-model';

@Injectable({
  providedIn: 'root'
})
export class AssetConvertService {
  public transaction: AssetConverssionViewModel;

  private hubConnection: signalR.HubConnection;

  constructor(private http: CustomHttpClientService, private auth: AuthService, private env: EnvironmentService) { }

  public async BeginSignalR(id: string): Promise<void> {
    const transaction = await this.GetTransaction(id);
    if (transaction.success)
      this.transaction = transaction.content;
    
    this.buildConnection();
    await this.hubConnection.start();
    this.listen();
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
      .withAutomaticReconnect()
      .withUrl(this.env.apiBaseUrl + "feed/assetconversion", options)
      .build();
  }

  private listen(): void {
    this.hubConnection?.on('assetconversiondata', (data) => {
      console.log("data received", data);
      this.transaction = data as AssetConverssionViewModel;
    })
  }

  public async GetTransactionLock(id: string): Promise<ApiResult<AssetConversionLockViewModel>> {
    return this.http.Get("AssetConvert/lock", { params: new HttpParams().set("id", id) });
  }
  
  public async LockTransaction(dto: AssetConversionLockDto): Promise<ApiResult<AssetConversionLockViewModel>> {
    return this.http.Post("AssetConvert/lock", dto);
  }

  public async RemoveTransactionLock(id: string): Promise<ApiResult> {
    return this.http.Delete("AssetConvert/lock", { params: new HttpParams().set("id", id) });
  }

  public async Convert(dto: AssetConverssionDto): Promise<ApiResult<string>> {
    return this.http.Post("AssetConvert/convert", dto);
  }

  public async GetTransaction(id: string): Promise<ApiResult<AssetConverssionViewModel>> {
    return this.http.Get("AssetConvert/transaction", { params: new HttpParams().set("id", id) });
  }

  public async GetTransactions(id: string): Promise<ApiResult<AssetConverssionViewModel[]>> {
    return this.http.Get("AssetConvert/transactions", { params: new HttpParams().set("id", id) });
  }
}
