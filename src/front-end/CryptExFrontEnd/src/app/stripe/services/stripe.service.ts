import { Injectable } from '@angular/core';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { CustomHttpClientService } from 'src/app/api/custom-http-client/custom-http-client.service';
import { ApiResult } from 'src/app/api/models/api-result';
import { EnvironmentService } from 'src/environments/service/environment.service';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  stripe: Stripe;

  constructor(private env: EnvironmentService, private httpClient: CustomHttpClientService) {
    this.InitStripe(this.env.stripePublicKey).then();
  }

  private async InitStripe(pubKey: string): Promise<void> {
    this.stripe = await loadStripe(pubKey, { locale: "auto" });
  }

  public async RedirectToCheckout(SessionId: string): Promise<void> {
    this.stripe.redirectToCheckout({ sessionId: SessionId })
  }
}
