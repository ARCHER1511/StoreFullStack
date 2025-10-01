import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthResponse } from '../interfaces/AuthResponse';
import { LoginRequest } from '../interfaces/LoginRequest';
import { RegisterRequest } from '../interfaces/RegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiBaseUrl}/Auth`;
  private loggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.loggedInSubject.asObservable();

  constructor(private http: HttpClient) { }

  register(dto: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, dto);
  }

  login(dto: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => {
        localStorage.setItem('accessToken', res.accessToken);
        localStorage.setItem('userId', res.userId);
        this.loggedInSubject.next(true);
      })
    );
  }
  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('userId');
    this.loggedInSubject.next(false);
  }
  // isLoggedIn(): boolean {
  //   return typeof window !== 'undefined' && !!localStorage.getItem('accessToken');
  // }
 setLoggedIn(status: boolean) {
  this.loggedInSubject.next(status);
}
  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }
  refresh(userId: string, refreshToken: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/refresh`, { userId, refreshToken });
  }
}
