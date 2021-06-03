import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SnackBar, SnackBarCreate } from '../components/snackbar/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  private showSnackbarEventSource = new Subject<SnackBar>();
  public ShowSnackbarEvent = this.showSnackbarEventSource.asObservable();
  private clearSnackbarsEventSource = new Subject<void>();
  public ClearSnackbarsEvent = this.clearSnackbarsEventSource.asObservable();

  constructor() { }

  public ShowSnackbar(snackBar: SnackBarCreate): void {
    this.showSnackbarEventSource.next(snackBar);
  }

  public ClearSnackBars(): void {
    this.clearSnackbarsEventSource.next();
  }

}
