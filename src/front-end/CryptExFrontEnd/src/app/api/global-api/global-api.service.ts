import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomHttpClientService } from '../custom-http-client/custom-http-client.service';
import { ApiResult } from '../models/api-result';

@Injectable({
  providedIn: 'root'
})
export class GlobalApiService {

  constructor(private http: CustomHttpClientService) { }

  /**
   * Check if the service is up
   * @returns "OK" if the service is up
   */
  public async HealthCheck(): Promise<ApiResult<string>> {
    return await this.http.Get<string>("ping");
  }
}
