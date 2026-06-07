import { TestBed } from '@angular/core/testing';

import { Architecture } from './architecture';

describe('Architecture', () => {
  let service: Architecture;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Architecture);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
