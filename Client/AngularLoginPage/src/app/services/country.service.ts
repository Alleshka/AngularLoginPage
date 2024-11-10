import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Country } from '../shared/domain/country';
import { Province } from '../shared/domain/province';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class CountryService {
    constructor(private http: HttpClient) { }

    getCountries() {
        return this.http.get<Country[]>(`${environment.baseUrl}/country/countries`);
    }

    getProvinces(countryId: string) {
        return this.http.get<Province[]>(`${environment.baseUrl}/country/${countryId}/provinces`);
    }
}