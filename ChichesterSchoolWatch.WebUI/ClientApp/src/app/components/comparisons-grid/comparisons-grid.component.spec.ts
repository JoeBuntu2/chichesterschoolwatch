import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComparisonsGridComponent } from './comparisons-grid.component';

describe('ComparisonsGridComponent', () => {
  let component: ComparisonsGridComponent;
  let fixture: ComponentFixture<ComparisonsGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComparisonsGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComparisonsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
