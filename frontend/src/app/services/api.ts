import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export type Product = {
  id: number;
  name: string;
  category: string;
  priceCents: number;
  active: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class Api {
  
  private baseUrl = 'http://localhost:5052';

  constructor(private http: HttpClient){}

  getProducts(search?: string, onlyActive?: boolean): Observable<Product[]> {
    let params = new HttpParams();

    if (search && search.trim().length > 0) {
      params = params.set('search', search.trim());
    }

    if (onlyActive === true ) {
      params = params.set('onlyActive', 'true');
    }

    return this.http.get<Product[]>(`${this.baseUrl}/products`, { params });
  }
}
