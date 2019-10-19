import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxRateIncreaseComponent } from './tax-rate-increase.component';

describe('TaxRateIncreaseComponent', () => {
  let component: TaxRateIncreaseComponent;
  let fixture: ComponentFixture<TaxRateIncreaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaxRateIncreaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxRateIncreaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
