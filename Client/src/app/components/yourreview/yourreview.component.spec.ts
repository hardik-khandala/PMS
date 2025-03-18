import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YourreviewComponent } from './yourreview.component';

describe('YourreviewComponent', () => {
  let component: YourreviewComponent;
  let fixture: ComponentFixture<YourreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YourreviewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YourreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
