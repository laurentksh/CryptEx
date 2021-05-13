import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { IEnvironment } from '../ienvironment';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService implements IEnvironment {

  constructor() { }

  get production(): boolean {
    return environment.production;
  }

  get baseUrl(): string {
    return environment.baseUrl;
  }

  get apiBaseUrl(): string {
    return environment.apiBaseUrl;
  }

  get stripePublicKey(): string {
    return environment.stripePublicKey;
  }
}
