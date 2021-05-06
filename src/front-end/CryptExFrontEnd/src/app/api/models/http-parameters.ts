import { HttpHeaders, HttpParams } from "@angular/common/http";

/**
 * HttpParameters, based on Angular's HTTP Client class.
 */
export interface HttpParameters {
    headers?: HttpHeaders | {
        [header: string]: string | string[];
    };
    observe?: 'body';
    params?: HttpParams | {
        [param: string]: string | string[];
    };
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
}
