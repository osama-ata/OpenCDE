import { Component, inject } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { AuthenticationService } from '../../services/authentication.service';
import { MatButton } from '@angular/material/button';
import {
  MatCard,
  MatCardContent,
  MatCardActions,
} from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'opencde-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatButton,
    MatCard,
    MatCardActions,
    MatCardContent,
    MatError,
    MatFormField,
    MatInput,
    MatLabel,
  ],
})
export class LoginComponent {
  private authenticationService = inject(AuthenticationService);

  loginForm: UntypedFormGroup;
  mode: 'login' | 'register' = 'login';
  isSubmitting = false;
  errorMessage: string | null = null;
  infoMessage: string | null = null;
  configured = this.authenticationService.isConfigured();

  constructor() {
    const formBuilder = inject(UntypedFormBuilder);

    this.loginForm = formBuilder.group({
      email: new UntypedFormControl('', [
        Validators.required,
        Validators.email,
      ]),
      password: new UntypedFormControl('', [
        Validators.required,
        Validators.minLength(6),
      ]),
    });
  }

  toggleMode(): void {
    this.mode = this.mode === 'login' ? 'register' : 'login';
    this.errorMessage = null;
    this.infoMessage = null;
  }

  submit(): void {
    if (!this.loginForm.valid || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;
    this.infoMessage = null;

    const { email, password } = this.loginForm.value;
    const request =
      this.mode === 'login'
        ? this.authenticationService.signInWithPassword(email, password)
        : this.authenticationService.signUpWithPassword(email, password);

    request.subscribe((result) => {
      this.isSubmitting = false;
      if (!result.success) {
        if (this.mode === 'register' && result.error?.includes('confirm')) {
          this.infoMessage = result.error;
        } else {
          this.errorMessage =
            result.error ?? 'Something went wrong, please try again.';
        }
      }
    });
  }
}
