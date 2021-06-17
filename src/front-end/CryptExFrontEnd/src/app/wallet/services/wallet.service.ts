import { Injectable } from '@angular/core';
import { ApiResult } from 'src/app/api/models/api-result';
import { WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { UserWalletViewModel } from '../models/user-wallet-view-model';
import { TotalViewModel } from '../models/total-view-model';

@Injectable({
  providedIn: 'root'
})
export class WalletService {

  constructor(private http: CustomHttpClientService) { }


  public async GetWalletList(): Promise<ApiResult<WalletViewModel[]>> {
    return this.http.Get("Wallets/list");
  }

  public async GetUserWallets(): Promise<ApiResult<UserWalletViewModel[]>> {
    return this.http.Get("Wallets");
  }

  public async GetTotal(): Promise<ApiResult<TotalViewModel>> {
    return this.http.Get("Wallets/total");
  }

  public async GetTotalFiat(): Promise<ApiResult<TotalViewModel>> {
    return this.http.Get("Wallets/total/fiat");
  }

  public async GetTotalCrypto(): Promise<ApiResult<TotalViewModel>> {
    return this.http.Get("Wallets/total/crypto");
  }
}
