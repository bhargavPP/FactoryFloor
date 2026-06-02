import { createAction, props } from '@ngrx/store';

import { LoginRequest } from '../../shared/models/login-request.model';
import { AuthResponse } from '../../shared/models/auth-response.model';

export const login = createAction(
  '[Auth] Login',
  props<{ loginRequest: LoginRequest }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ authResponse: AuthResponse }>()
);

export const loginFailure = createAction(

  '[Auth] Login Failure',
  props<{ error: string }>()
);
export const logout = createAction('[Auth] Logout');

