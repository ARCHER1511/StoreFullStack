import { Observable } from 'rxjs';
import { AuthResponse } from './AuthResponse';
import { LoginRequest } from './LoginRequest';
import { RegisterRequest } from './RegisterRequest';

export interface IAuthService {
  register(dto: RegisterRequest): Observable<AuthResponse>;
  login(dto: LoginRequest): Observable<AuthResponse>;
  refresh(userId: string, refreshToken: string): Observable<AuthResponse>;
}