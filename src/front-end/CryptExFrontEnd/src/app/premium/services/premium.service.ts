import { Injectable } from '@angular/core';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { BuyPremiumDto } from '../models/buy-premium-dto';

@Injectable({
  providedIn: 'root'
})
export class PremiumService {

  constructor(private http: CustomHttpClientService) { }

  public async BuyPremium(dto: BuyPremiumDto): Promise<ApiResult> {
    return this.http.Post("User/premium", dto);
  }

  public async CancelPremium(): Promise<ApiResult> {
    return this.http.Delete("User/premium");
  }
}
