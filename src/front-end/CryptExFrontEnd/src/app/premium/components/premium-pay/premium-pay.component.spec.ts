import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PremiumPayComponent } from './premium-pay.component';

describe('PremiumPayComponent', () => {
  let component: PremiumPayComponent;
  let fixture: ComponentFixture<PremiumPayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PremiumPayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PremiumPayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
