import { Injectable } from '@angular/core';
import { ApiResult } from 'src/app/api/models/api-result';
import { DepositViewModel } from 'src/app/deposit-withdraw/models/deposit-view-model';
import { WalletViewModel } from 'src/app/user/models/wallet-view-model';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class WalletService {

  constructor(private http: CustomHttpClientService) { }


  public async GetWalletList(): Promise<ApiResult<WalletViewModel[]>> {
    return this.http.Get<WalletViewModel[]>("Wallets/list");
  }

}
