import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewTransactionComponent } from './preview-transaction.component';

describe('PreviewTransactionComponent', () => {
  let component: PreviewTransactionComponent;
  let fixture: ComponentFixture<PreviewTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviewTransactionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
