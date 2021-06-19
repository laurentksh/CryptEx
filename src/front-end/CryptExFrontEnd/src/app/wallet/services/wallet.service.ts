import { Injectable } from '@angular/core';
import { ApiResult } from 'src/app/api/models/api-result';
import { WalletViewModel } from 'src/app/wallet/models/wallet-view-model';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { UserWalletViewModel } from '../models/user-wallet-view-model';
import { TotalsViewModel } from '../models/totals-view-model';

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

  public async GetTotals(): Promise<ApiResult<TotalsViewModel>> {
    return this.http.Get("Wallets/totals");
  }
}
