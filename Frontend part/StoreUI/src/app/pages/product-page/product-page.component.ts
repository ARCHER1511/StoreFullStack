import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/Product';
import { NgForOf } from '@angular/common';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-products',
  templateUrl: './product-page.component.html',
  styleUrl: './product-page.component.scss',
  imports: [NgForOf, RouterLink]
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe(res => {
      this.products = res.data;
    });
  }

  getImagePath(imagePath: string): string {
    return this.productService.getImagePath(imagePath);
  }
  deleteProduct(id: string): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          this.products = this.products.filter(p => p.id !== id);
        },
        error: () => {
          alert('Failed to delete product');
        }
      });
    }
  }
}