import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfReviewComponent } from './self-review.component';

describe('SelfReviewComponent', () => {
  let component: SelfReviewComponent;
  let fixture: ComponentFixture<SelfReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SelfReviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelfReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
