import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PromiumPricingComponent } from './promium-pricing.component';

describe('PromiumPricingComponent', () => {
  let component: PromiumPricingComponent;
  let fixture: ComponentFixture<PromiumPricingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PromiumPricingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PromiumPricingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
