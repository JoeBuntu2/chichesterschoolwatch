import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExcessChichesterSpendingComponent } from './excess-chichester-spending.component';

describe('ExcessChichesterSpendingComponent', () => {
  let component: ExcessChichesterSpendingComponent;
  let fixture: ComponentFixture<ExcessChichesterSpendingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExcessChichesterSpendingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExcessChichesterSpendingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
