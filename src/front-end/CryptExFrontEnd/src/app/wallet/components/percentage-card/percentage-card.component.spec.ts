import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PercentageCardComponent } from './percentage-card.component';

describe('PercentageCardComponent', () => {
  let component: PercentageCardComponent;
  let fixture: ComponentFixture<PercentageCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PercentageCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PercentageCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
