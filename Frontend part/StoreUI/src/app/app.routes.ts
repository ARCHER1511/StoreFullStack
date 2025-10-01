import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ProductDetailsPageComponent } from './pages/product-details-page/product-details-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { NgModule } from '@angular/core';
import { AuthGuard } from './auth-guard.guard';
import { ProductsComponent } from './pages/product-page/product-page.component';
import { ProductFormPageComponent } from './pages/product-form-page/product-form-page.component';
import { AddProductPageComponent } from './pages/add-product-page/add-product-page.component';

export const routes: Routes =
  [
    { path: 'home', component: HomeComponent },
    { path: 'products', component: ProductsComponent, canActivate: [AuthGuard] },
    { path: 'products/create', component: AddProductPageComponent, canActivate: [AuthGuard] },
    { path: 'products/:id', component: ProductDetailsPageComponent, canActivate: [AuthGuard] },
    { path: 'products/edit/:id', component: ProductFormPageComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent },

    // Redirect empty path to home
    { path: '', redirectTo: '/home', pathMatch: 'full' },

    // Wildcard route for 404 page
    { path: '**', redirectTo: '/home' }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }