import { Inject, Injectable, inject } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';

import { Router } from '@angular/router';

import {
  catchError,
  map,
  of,
  switchMap,
  tap
} from 'rxjs';

import * as AuthActions from './auth.actions';

import { AuthService } from '../../services/auth.service';

@Injectable()
export class AuthEffects {

  private actions$ = inject(Actions);

  private authService = inject(AuthService);

  private router = inject(Router);

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(actions =>
        this.authService.login(actions.loginRequest).pipe(
          map(authResponse => AuthActions.loginSuccess({ authResponse })),

          catchError(error => of(AuthActions.loginFailure({ error: error.message }))
          )
        )
      )
    )
  );
  loginSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginSuccess),
      tap(action => {
        localStorage.setItem(
          'token',
          action.authResponse.token
        );

        this.router.navigate(['/']);
      })
    ),
    { dispatch: false }
  );
  logout$ = createEffect(
    () =>
      this.actions$.pipe(

        ofType(AuthActions.logout),

        tap(() => {

          localStorage.removeItem('token');

          this.router.navigate(['/login']);
        })
      ),
    { dispatch: false }
  );

}
