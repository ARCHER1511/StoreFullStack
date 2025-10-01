import { Observable } from 'rxjs';
import { Product } from './Product';
import { GeneralResponse } from './GeneralResponse';

export interface IProductService {
  getProducts(): Observable<GeneralResponse<Product[]>>;
  getProductById(id: string): Observable<Product>;
  addProduct(product: Product): Observable<Product>;
  updateProduct(id: string, product: Product): Observable<Product>;
  deleteProduct(id: string): Observable<void>;
}