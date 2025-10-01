import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Product } from '../interfaces/Product';
import { GeneralResponse } from '../interfaces/GeneralResponse';
import { ProductCreateUpdateDto } from '../interfaces/ProductCreateUpdateDto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiBaseUrl}/Products`;
  private imagesUrl = environment.imagesBaseUrl;

  constructor(private http: HttpClient) { }

  // ✅ Helper to get headers with token
  private getAuthHeaders(): HttpHeaders {
    let token = '';
    if (typeof window !== 'undefined') {
      token = localStorage.getItem('accessToken') ?? '';
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      Accept: 'application/json'
    });
  }

  getProducts(): Observable<GeneralResponse<Product[]>> {
    return this.http.get<GeneralResponse<Product[]>>(this.apiUrl, {
      headers: this.getAuthHeaders()
    });
  }

  getProductById(id: string): Observable<GeneralResponse<Product>> {
    return this.http.get<GeneralResponse<Product>>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  getImagePath(imagePath: string): string {
    return `${this.imagesUrl}/${imagePath}`;
  }

  addProduct(product: ProductCreateUpdateDto): Observable<Product> {
    const formData = this.buildFormData(product);
    return this.http.post<Product>(this.apiUrl, formData, {
      headers: this.getAuthHeadersForForm()
    });
  }

  updateProduct(id: string, product: ProductCreateUpdateDto): Observable<Product> {
    const formData = this.buildFormData(product);
    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData, {
      headers: this.getAuthHeadersForForm()
    });
  }

  deleteProduct(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }



  // ✅ Helper to build FormData
  private buildFormData(product: ProductCreateUpdateDto): FormData {
    const form = new FormData();
    form.append('category', product.category);
    form.append('name', product.name);
    form.append('price', product.price.toString());
    form.append('minimumQuantity', product.minimumQuantity.toString());
    form.append('discountRate', product.discountRate.toString());
    if (product.image) form.append('image', product.image);
    return form;
  }

  // ✅ FormData needs different headers
  private getAuthHeadersForForm(): HttpHeaders {
    let token = '';
    if (typeof window !== 'undefined') {
      token = localStorage.getItem('accessToken') ?? '';
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
      // Don't set Content-Type manually for FormData
    });
  }
}