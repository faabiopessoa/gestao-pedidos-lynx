import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export type Product = {
  id: number;
  name: string;
  category: string;
  priceCents: number;
  active: boolean;
};

export type OrderSummary = {
  id: number;
  customerId: number;
  customerName: string;
  status: string;
  createdAt: string;
  itemsCount: number;
  totalCents: number;
};

export type OrderDetail = {
  id: number;
  customerId: number;
  customerName: string;
  status: string;
  createdAt: string;
  totalCents: number;
  paidCents: number;
  remainingCents: number;
  items: {
    productId: number;
    quantity: number;
    unitPriceCents: number;
  }[];
};

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

  createOrder(payload: {
    customerId: number; items: {productId: number; quantity: number}[];
  }) {
    return this.http.post<{order_id: number}>(
      `${this.baseUrl}/orders`, payload
    );
  }

  getOrders() {
    return this.http.get<OrderSummary[]>(`${this.baseUrl}/orders`);
  }

  getOrdersById(id: number) {
    return this.http.get<OrderDetail>(`${this.baseUrl}/orders/${id}`);
  }
}
