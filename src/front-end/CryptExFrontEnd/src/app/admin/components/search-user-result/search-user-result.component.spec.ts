import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchUserResultComponent } from './search-user-result.component';

describe('SearchUserResultComponent', () => {
  let component: SearchUserResultComponent;
  let fixture: ComponentFixture<SearchUserResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchUserResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchUserResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
