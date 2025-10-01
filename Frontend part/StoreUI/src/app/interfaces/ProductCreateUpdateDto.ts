export interface ProductCreateUpdateDto {
  category: string;
  name: string;
  image?: File | null;   // maps to IFormFile in .NET
  price: number;
  minimumQuantity: number;
  discountRate: number;
}