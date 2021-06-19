import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PremiumManageComponent } from './premium-manage.component';

describe('PremiumManageComponent', () => {
  let component: PremiumManageComponent;
  let fixture: ComponentFixture<PremiumManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PremiumManageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PremiumManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
