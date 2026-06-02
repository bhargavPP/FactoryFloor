export interface AuthState {

  token: string | null;

  loading: boolean;

  error: string | null;
}
export const initialAuthState: AuthState = {

  token: null,

  loading: false,

  error: null
};
