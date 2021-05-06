import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from '../custom-http-client/custom-http-client.service';
import { ApiResult } from '../models/api-result';

@Injectable({
  providedIn: 'root'
})
export class GlobalApiService {

  constructor(private http: CustomHttpClientService) { }

  public async GetApiResponseTime(): Promise<ApiResult<number>> {
    return await this.http.Get<number>("ping");
  }
}
