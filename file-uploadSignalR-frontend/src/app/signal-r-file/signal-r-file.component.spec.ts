import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignalRFileComponent } from './signal-r-file.component';

describe('SignalRFileComponent', () => {
  let component: SignalRFileComponent;
  let fixture: ComponentFixture<SignalRFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignalRFileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignalRFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
