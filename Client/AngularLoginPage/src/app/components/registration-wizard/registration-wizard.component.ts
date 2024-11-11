import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrationService } from 'src/app/services/registration.service';
import { CountryService } from 'src/app/services/country.service';
import { UserRegister } from 'src/app/shared/domain/user';
import { Country } from 'src/app/shared/domain/country';
import { Province } from 'src/app/shared/domain/province';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration-wizard',
  templateUrl: './registration-wizard.component.html',
  styleUrls: ['./registration-wizard.component.scss']
})
export class RegistrationWizardComponent implements OnInit {
  registrationForm: FormGroup;
  currentStep: number = 1;
  countries: Country[] = [];
  provinces: Province[] = [];
  submitted: boolean = false;
  error: string = "";

  constructor(
    private fb: FormBuilder,
    private registrationService: RegistrationService,
    private countryService: CountryService,
    private router: Router
  ) {
    this.registrationForm = this.fb.group({
      step1: this.fb.group({
        login: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.pattern(/^(?=.*[\p{L}])(?=.*\d).{2,}$/u)]],
        confirmPassword: ['', Validators.required],
        agree: [false, Validators.requiredTrue]
      }, { validators: this.matchPasswords }),
      step2: this.fb.group({
        country: ['', Validators.required],
        province: ['', Validators.required]
      })
    });
  }

  get step1Form(): FormGroup {
    return this.registrationForm.get('step1') as FormGroup;
  }

  get step2Form(): FormGroup {
    return this.registrationForm.get('step2') as FormGroup;
  }

  ngOnInit(): void {
    this.countryService.getCountries().subscribe(data => {
      this.countries = data;
    });
  }

  private matchPasswords(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;

    return password === confirmPassword ? null : { passwordsMismatch: true };
  }

  nextStep() {
    if (this.currentStep === 1 && this.registrationForm.get('step1')?.valid) {
      this.currentStep = 2;
    }
  }

  prevStep() {
    if (this.currentStep === 2) {
      this.currentStep = 1;
    }
  }

  onCountryChange(target: EventTarget | null) {
    const option = target as HTMLInputElement;
    if (option) {
      const country = option.value;
      if (country) {
        if (!this.submitted) {
          this.submitted = true;
          this.countryService.getProvinces(country).subscribe(data => {
            this.provinces = data;
            this.registrationForm.get('step2')?.get('province')?.reset();
            this.submitted = false;
          }, () => this.submitted = false)
        }
      }
      else {
        this.provinces = [];
        this.registrationForm.get('step2')?.get('province')?.reset();
      }
    }
  }

  save() {
    if (this.registrationForm.get('step2')?.invalid) {
      return;
    }

    const user: UserRegister = {
      login: this.step1Form.value.login,
      password: this.step1Form.value.password,
      confirmPassword: this.step1Form.value.confirmPassword,
      provinceId: this.step2Form.value.province
    };

    this.registrationService.register(user).subscribe(res => {
      this.registrationForm.reset();
      this.submitted = false;
      this.router.navigate(['/']);
      this.currentStep = 1;
    }, (error) => {
      this.error = error.error;
      console.log(error);
    })
  }
}
