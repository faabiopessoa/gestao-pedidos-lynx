import { Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api, OrderSummary, OrderDetail } from '../../services/api';

@Component({
  selector: 'app-orders',
  imports: [CommonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders implements OnInit {
  orders: OrderSummary[] = [];
  selected: OrderDetail | null = null;
  error = '';

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
      this.loadOrders();
  }

  loadOrders() {
    this.error = '';
    this.selected = null;

    this.api.getOrders().subscribe({
      next: (data) => {
        this.orders = data.sort((a, b) => b.id - a.id);
        Promise.resolve().then(() => this.cdr.detectChanges());
      },
      error: () => {
        this.error = 'Erro ao carregar pedidos.';
        Promise.resolve().then(() => this.cdr.detectChanges());
      }
    });
  }

  open(id: number) {
    this.error = '';
    this.selected = null;

    this.api.getOrdersById(id).subscribe({
      next: (data) => {
        this.selected = data;
        Promise.resolve().then(() => this.cdr.detectChanges());
      }, error: () => {
        this.error = 'Erro ao carregar detalhes do pedido.';
        Promise.resolve().then(() => this.cdr.detectChanges());
      }
    });
  }

  close() {
    this.selected = null;
  }

  itemSubtotal(item: { quantity: number; unitPriceCents: number}) {
    return item.quantity * item.unitPriceCents;
  }

  trackById(_: number, x: {id: number}) {
    return x.id;
  }
}
