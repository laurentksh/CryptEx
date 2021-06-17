import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PromiumPricingComponent } from './Components/promium-pricing/promium-pricing.component';
import {PremiumRouting} from "./premium-routing";



@NgModule({
  declarations: [
    PromiumPricingComponent
  ],
  imports: [
    CommonModule,
    PremiumRouting
  ]
})
export class PremiumModule { }
