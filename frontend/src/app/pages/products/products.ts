import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api, Product } from '../../services/api';

type Cart = {
  productId: number;
  name: string;
  category: string;
  unitPriceCents: number;
  quantity: number;
};

@Component({
  selector: 'app-products',
  imports: [CommonModule,FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit {

  search = '';
  products: Product[] = [];
  error = '';
  onlyActive = false;
  categories: string [] = [];
  selectedCategory = '';
  cart: Cart[] = [];

  orderError = '';
  createdOrderId: number | null = null;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
      this.loadProducts();
  }

  Categories(fromProducts: Product[]) {
    const set = new Set<string>();

    for (const p of fromProducts) {
      const cat = (p.category ?? '').trim();
      if (cat.length > 0) {
        set.add(cat);
      }
    }

    this.categories = Array.from(set).sort((a,b) => a.localeCompare(b));

    if (this.selectedCategory && !this.categories.includes(this.selectedCategory)) {
      this.selectedCategory = '';
    }
  }

  get filteredProducts(): Product[] {
    if (!this.selectedCategory) return this.products;
    return this.products.filter(p => p.category === this.selectedCategory);
  }

  loadProducts() {
    this.error = '';

    this.api.getProducts(this.search, this.onlyActive).subscribe({
      next: (data) => {
        this.products = data.sort((a,b) => a.id - b.id);
        this.Categories(this.products);
        Promise.resolve().then(() => this.cdr.detectChanges());
      },
      error: () => {
        this.error = 'Erro ao carregar produtos.';
        Promise.resolve().then(() => this.cdr.detectChanges());
      }
    });
  }

  onSearch() {
    this.loadProducts();
  }

  onClear() {
    this.search = '';
    this.onlyActive = false;
    this.selectedCategory = '';
    this.loadProducts();
  }

  onOnlyActiveChange() {
    this.loadProducts();
  }

  addToCart(p: Product) {
    if (!p.active) return;
    this.createdOrderId = null;

    const existing = this.cart.find(i => i.productId === p.id);
    if (existing) {
      existing.quantity += 1;
      return;
    }

    this.cart.push({
      productId: p.id,
      name: p.name,
      category: p.category,
      unitPriceCents: p.priceCents,
      quantity: 1,
    });
  }

  increase(item: Cart) {
    item.quantity += 1;
  }

  decrease(item: Cart) {
    if (item.quantity <= 1) {
      this.remove(item);
      return;
    }
    item.quantity -= 1;
  }

  remove(item: Cart) {
    this.cart = this.cart.filter(i => i.productId !== item.productId);
  }

  clearCart() {
    this.cart = [];
  }

  cartSubtotal(item: Cart): number {
    return item.unitPriceCents * item.quantity;
  }

  get cartTotal(): number {
    return this.cart.reduce((sum, i) => sum + (i.unitPriceCents * i.quantity), 0);
  }

  finalizeOrder() {
    this.orderError = '';
    this.createdOrderId = null;

    const payload = {
      customerId: 1,
      items: this.cart.map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };

    this.api.createOrder(payload).subscribe({
      next: (res: any) => {
        this.createdOrderId = Number(res.order_id);

        Promise.resolve().then(() => {
          this.cdr.detectChanges();
          this.clearCart();
          this.cdr.detectChanges();
        })
      }
    });
  }

}
