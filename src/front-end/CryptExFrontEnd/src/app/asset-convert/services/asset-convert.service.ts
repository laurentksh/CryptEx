import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { AssetConvertDto } from '../models/asset-convert-dto';
import * as signalR from "@microsoft/signalr";
import { AssetConvertViewModel } from '../models/asset-convert-view-model';
import { AuthService } from 'src/app/auth/services/auth.service';
import { EnvironmentService } from 'src/environments/service/environment.service';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AssetConvertService {
  public transaction: AssetConvertViewModel;

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
      this.transaction = data as AssetConvertViewModel;
    })
  }

  public async Convert(dto: AssetConvertDto): Promise<ApiResult<string>> {
    return this.http.Post("AssetConvert/convert", dto);
  }

  public async GetTransaction(id: string): Promise<ApiResult<AssetConvertViewModel>> {
    return this.http.Get("AssetConvert/transaction", { params: new HttpParams().set("id", id) });
  }
}
