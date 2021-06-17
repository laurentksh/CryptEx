import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionStatusPageComponent } from './transaction-status-page.component';

describe('TransactionStatusPageComponent', () => {
  let component: TransactionStatusPageComponent;
  let fixture: ComponentFixture<TransactionStatusPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransactionStatusPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransactionStatusPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
