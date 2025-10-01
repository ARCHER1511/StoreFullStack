import { CommonModule } from "@angular/common";
import { ProductCreateUpdateDto } from "../../interfaces/ProductCreateUpdateDto";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { ProductService } from "../../services/product.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-product-form-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form-page.component.html',
  styleUrls: ['./product-form-page.component.scss']
})
export class ProductFormPageComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  productId: string | null = null;
  selectedImage: File | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private productService: ProductService,
    private router: Router
  ) {
    this.form = this.fb.group({
      category: ['', Validators.required],
      name: ['', Validators.required],
      price: [0, Validators.required],
      minimumQuantity: [1, Validators.required],
      discountRate: [0, Validators.required],
      image: [null]
    });
  }

  ngOnInit(): void {
  const currentRoute = this.route.snapshot.routeConfig?.path;
  this.isEditMode = this.route.snapshot.routeConfig?.path?.includes('edit') ?? false;

  if (this.isEditMode) {
    this.productId = this.route.snapshot.paramMap.get('id');
    this.productService.getProductById(this.productId!).subscribe({
      next: (res) => {
        const p = res.data;
        this.form.patchValue({
          category: p.category,
          name: p.name,
          price: p.price,
          minimumQuantity: p.minimumQuantity,
          discountRate: p.discountRate
        });
      }
    });
  }
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
      ...this.form.value,
      image: this.selectedImage
    };

    const request = this.isEditMode
      ? this.productService.updateProduct(this.productId!, dto)
      : this.productService.addProduct(dto);

    request.subscribe({
      next: () => this.router.navigate(['/products']),
      error: () => alert('Failed to save product')
    });
  }
}