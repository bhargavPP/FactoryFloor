import { Component, inject } from '@angular/core';

import { FormsModule } from '@angular/forms';

import { Store } from '@ngrx/store';

import { MaterialModule }
  from '../../shared/material/material.module';

import * as AuthActions
  from '../../store/auth/auth.actions';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,
    MaterialModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private store = inject(Store);

  email = '';

  password = '';

  login(): void {
    this.store.dispatch(
      AuthActions.login({
        loginRequest: {
          email: this.email,
          password: this.password
        }
      })
    );  
  }
}
