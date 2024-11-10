import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationWizardComponent } from './registration-wizard.component';

describe('RegistrationWizardComponent', () => {
  let component: RegistrationWizardComponent;
  let fixture: ComponentFixture<RegistrationWizardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistrationWizardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
