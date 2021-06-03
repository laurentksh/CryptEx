import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvironmentService } from 'src/environments/service/environment.service';
import { ApiResult } from '../models/api-result';
import { HttpParameters } from '../models/http-parameters';

@Injectable({
  providedIn: 'root'
})
export class CustomHttpClientService {

  constructor(public httpClient: HttpClient, public env: EnvironmentService) { }

  /**
   * Send GET Request and parse the result as T
   * @param relativeUrl Relative URL
   * @param parameters Request parameters
   * @returns Promise containing an ApiResult<T> object.
   */
  public async Get<T = void>(relativeUrl: string, parameters?: HttpParameters): Promise<ApiResult<T>> {
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

  /**
   * Send POST Request with a body and parse the result as T
   * @param relativeUrl 
   * @param body 
   * @param parameters 
   * @returns Promise containing an ApiResult<T> object.
   */
  public async Post<T = void>(relativeUrl: string, body: any, parameters?: HttpParameters): Promise<ApiResult<T>> {
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

  /**
   * Send DELETE Request
   * @param relativeUrl 
   * @param parameters 
   * @returns Promise containing an ApiResult<void> object (the result is not parsed).
   */
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
    if (relativeUrl.startsWith("/")) {
      return this.env.apiBaseUrl.substr(0, this.env.apiBaseUrl.length - 1) + relativeUrl;
    } else {
      return this.env.apiBaseUrl + "api/" + relativeUrl;
    }
  }
  //#endregion
}
