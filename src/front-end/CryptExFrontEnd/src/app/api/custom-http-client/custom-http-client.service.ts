import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvironmentService } from 'src/environments/service/environment.service';
import { ApiResult } from './models/api-result';
import { HttpParameters } from './models/http-parameters';

@Injectable({
  providedIn: 'root'
})
export class CustomHttpClientService {

  constructor(public httpClient: HttpClient, public env: EnvironmentService) { }

  /**
   * Send GET Request
   * @param relativeUrl Relative URL
   * @param parameters Request parameters
   * @returns Promise with type @type
   */
  public async Get<T>(relativeUrl: string, parameters?: HttpParameters): Promise<ApiResult<T>> {
    const result = {} as ApiResult<T>;
    result.success = true;
    
    try {
      result.content = await this.httpClient.get<T>(this.ConcatUrl(relativeUrl), parameters).toPromise();
    } catch (ex) {
      result.error = ex;
      result.success = false;
    }

    return result;
  }

  public async Post<T>(relativeUrl: string, body: any, parameters?: HttpParameters): Promise<ApiResult<T>> {
    const result = {} as ApiResult<T>;
    result.success = true;
    
    try {
      result.content = await this.httpClient.post<T>(this.ConcatUrl(relativeUrl), body, parameters).toPromise();
    } catch (ex) {
      result.error = ex;
      result.success = false;
    }
    
    return result;
  }

  public async Delete(relativeUrl: string, parameters?: HttpParameters): Promise<ApiResult> {
    const result = {} as ApiResult;
    result.success = true;
    
    try {
      await this.httpClient.delete(this.ConcatUrl(relativeUrl), parameters).toPromise();
    } catch (ex) {
      result.error = ex;
      result.success = false;
    }
    
    return result;
  }

  //#region Utilities
  public ConcatUrl(relativeUrl: string): string {
    return this.env.apiBaseUrl + relativeUrl;
  }
  //#endregion
}
