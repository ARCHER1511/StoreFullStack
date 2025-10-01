import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/Product';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-product-details-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-details-page.component.html',
  styleUrls: ['./product-details-page.component.scss']
})
export class ProductDetailsPageComponent implements OnInit {
  product: Product | null = null;
  errorMessage = '';
  isLoading = true;

  constructor(private route: ActivatedRoute, private productService: ProductService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');
        console.log('Product ID from route:', id); // ✅ Debug
        if (!id) return of(null);
        return this.productService.getProductById(id);
      })
    ).subscribe({
      next: (res) => {
        console.log('Product data received:', res); // ✅ Debug
        this.product = res?.data || null;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching product:', err); // ✅ Debug
        this.errorMessage = 'Product not found or unauthorized';
        this.isLoading = false;
      }
    });

  }

  getImageUrl(): string {
    return this.product ? this.productService.getImagePath(this.product.imagePath) : '';
  }
}