import { AppConfigService } from './app-config.service';
import { Injectable, inject } from '@angular/core';
import { JwtTokenService } from '@dangl/angular-dangl-identity-client';
import { Observable, from } from 'rxjs';
import { Router } from '@angular/router';
import { SupabaseClient, createClient } from '@supabase/supabase-js';
import { map } from 'rxjs/operators';

export interface AuthResult {
  success: boolean;
  error?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private appConfigService = inject(AppConfigService);
  private jwtTokenService = inject(JwtTokenService);
  private router = inject(Router);

  private supabaseClient: SupabaseClient | null = null;

  /** True once the server has a Supabase project configured for this UI to sign in against. */
  isConfigured(): boolean {
    const config = this.appConfigService.getFrontendConfig();
    return !!(config?.supabaseUrl && config?.supabaseAnonKey);
  }

  signInWithPassword(email: string, password: string): Observable<AuthResult> {
    return from(
      this.getSupabaseClient().auth.signInWithPassword({ email, password })
    ).pipe(map((response) => this.handleSessionResponse(response)));
  }

  signUpWithPassword(email: string, password: string): Observable<AuthResult> {
    return from(this.getSupabaseClient().auth.signUp({ email, password })).pipe(
      map((response) => this.handleSessionResponse(response))
    );
  }

  private handleSessionResponse(response: {
    data: { session: { access_token: string; expires_at?: number } | null };
    error: { message: string } | null;
  }): AuthResult {
    if (response.error) {
      return { success: false, error: response.error.message };
    }

    const session = response.data.session;
    if (!session) {
      // Happens on sign-up when Supabase requires email confirmation before
      // issuing a session: there's no token to store yet.
      return {
        success: false,
        error: 'Please check your email to confirm your account, then log in.',
      };
    }

    // We're just storing the access token, there's not going to be any
    // refresh token functionality -- the user signs in again once it expires.
    this.jwtTokenService.storeCustomToken({
      accessToken: session.access_token,
      expiresAt: session.expires_at ?? Math.floor(Date.now() / 1000) + 3600,
    });
    this.router.navigateByUrl('/');

    return { success: true };
  }

  private getSupabaseClient(): SupabaseClient {
    if (this.supabaseClient) {
      return this.supabaseClient;
    }

    const config = this.appConfigService.getFrontendConfig();
    if (!config?.supabaseUrl || !config?.supabaseAnonKey) {
      throw new Error(
        'This OpenCDE server has no Supabase project configured (Supabase:AnonKey missing) -- sign-in is disabled.'
      );
    }

    this.supabaseClient = createClient(
      config.supabaseUrl,
      config.supabaseAnonKey
    );
    return this.supabaseClient;
  }
}
