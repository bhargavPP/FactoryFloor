import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';

import { AuthState, initialAuthState } from './auth.state';

export const authReducer = createReducer(
  initialAuthState,

  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(AuthActions.loginSuccess, (state, { authResponse }) => ({
    ...state,
    token: authResponse.token,
    loading: false,
    error: null
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error: error
  })),
  on(AuthActions.logout, () => ({
    ...initialAuthState
  }))
);
