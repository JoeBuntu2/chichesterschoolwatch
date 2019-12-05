import { TestBed } from '@angular/core/testing';

import { DistrictNameSorterService } from './district-name-sorter.service';

describe('DistrictNameSorterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DistrictNameSorterService = TestBed.get(DistrictNameSorterService);
    expect(service).toBeTruthy();
  });
});
