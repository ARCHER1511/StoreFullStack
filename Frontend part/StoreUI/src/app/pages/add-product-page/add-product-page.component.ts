import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { ProductCreateUpdateDto } from '../../interfaces/ProductCreateUpdateDto';

@Component({
  selector: 'app-add-product-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-product-page.component.html',
  styleUrls: ['./add-product-page.component.scss']
})
export class AddProductPageComponent {
  form: FormGroup;
  selectedImage: File | null = null;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private router: Router
  ) {
    this.form = this.fb.group({
      category: ['', Validators.required],
      name: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      minimumQuantity: [1, [Validators.required, Validators.min(1)]],
      discountRate: [0, [Validators.required, Validators.min(0)]],
      image: [null]
    });
  }

  onFileChange(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.selectedImage = file;
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const dto: ProductCreateUpdateDto = {
      category: this.form.value.category || '',
      name: this.form.value.name || '',
      price: this.form.value.price ?? 0,
      minimumQuantity: this.form.value.minimumQuantity ?? 1,
      discountRate: this.form.value.discountRate ?? 0,
      image: this.selectedImage
    };

    this.productService.addProduct(dto).subscribe({
      next: () => this.router.navigate(['/products']),
      error: () => alert('Failed to create product')
    });
  }
}