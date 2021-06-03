import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SnackbarService } from 'src/app/services/snackbar.service';
import { AlertType, SnackBarUI } from './snack-bar';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.scss']
})
export class SnackbarComponent implements OnInit {
  public readonly AlertTypeRef = AlertType; //Reference so we can use it in the HTML

  public SnackBars: SnackBarUI[];

  private newSnackbarSub: Subscription;
  private clearSnackbarsSub: Subscription;

  constructor(private snackBarService: SnackbarService, private cdRef: ChangeDetectorRef, private router: Router) {
    this.SnackBars = new Array<SnackBarUI>();
  }

  ngOnInit(): void {
    this.newSnackbarSub = this.snackBarService.ShowSnackbarEvent.subscribe(x => {
      const snack = x as SnackBarUI;
      this.SnackBars.push(snack);
      this.cdRef.detectChanges();

      this.showSnackBar(snack);
      if (x.CloseAfter != null && x.CloseAfter != 0)
        this.closeAfter(snack, x.CloseAfter);
    });

    this.clearSnackbarsSub = this.snackBarService.ClearSnackbarsEvent.subscribe(x => {
      this.SnackBars.forEach(snackbar => {
        this.removeSnackBar(snackbar);
      });
    });

    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.SnackBars.forEach(snackbar => {
          this.removeSnackBar(snackbar);
        });
      }
    });
  }

  onContinue(snackbar: SnackBarUI): void {
    if (snackbar.OnContinueActionCallback != null)
      snackbar.OnContinueActionCallback();
    
    this.removeSnackBar(snackbar);
  }

  onPrimary(snackbar: SnackBarUI): void {
    if (snackbar.OnPrimaryActionCallback != null)
      snackbar.OnPrimaryActionCallback();
    
    this.removeSnackBar(snackbar);
  }

  onSecondary(snackbar: SnackBarUI): void {
    if (snackbar.OnSecondaryActionCallback != null)
      snackbar.OnSecondaryActionCallback();
    
    this.removeSnackBar(snackbar);
  }

  onClose(snackbar: SnackBarUI): void {
    if (snackbar.OnCloseActionCallback != null)
      snackbar.OnCloseActionCallback();
    
    this.removeSnackBar(snackbar);
  }

  async showSnackBar(snackbar: SnackBarUI): Promise<void> {
    await this.wait(50);

    snackbar.Hide = false;
    snackbar.Show = true;
  }

  async removeSnackBar(snackbar: SnackBarUI): Promise<void> {
    snackbar.Show = false;
    snackbar.Hide = true;

    await this.wait(300);

    const index = this.SnackBars.indexOf(snackbar);

    if (index == -1)
      return;
    
    this.SnackBars.splice(index, 1);
  }

  ngOnDestroy(): void {
    this.newSnackbarSub.unsubscribe();
  }

  async closeAfter(snack: SnackBarUI, ms: number): Promise<void> {
    await this.wait(ms);

    this.removeSnackBar(snack);
  }

  private async wait(ms: number): Promise<void> {
    await new Promise(res => setTimeout(res, ms));
  }
}