import { HttpErrorResponse } from '@angular/common/http';

export interface ApiResult<T = void> {
    success: boolean;
    content: T
    error: HttpErrorResponse;
}
