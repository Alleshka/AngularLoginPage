import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegister } from '../shared/domain/user';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
    constructor(private http: HttpClient) {}

    register(user: UserRegister) {
        return this.http.post<UserRegister>(`${environment.baseUrl}/user/register`, user);
    }
}