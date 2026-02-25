import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameService, ItemDto, CharacterDto } from '../../game.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit {
  items: ItemDto[] = [];
  character: CharacterDto | null = null;
  message = '';
  error = '';
  filter = 'all';

  constructor(private gameService: GameService, private router: Router) {}

  ngOnInit(): void {
    this.gameService.character$.subscribe(c => this.character = c);
    this.loadItems();
  }

  loadItems(): void {
    this.gameService.getShopItems().subscribe({
      next: (items) => this.items = items,
      error: () => this.error = 'Nie udało się załadować sklepu.'
    });
  }

  get filteredItems(): ItemDto[] {
    if (this.filter === 'all') return this.items;
    return this.items.filter(i => i.type === this.filter);
  }

  buy(itemId: number): void {
    this.message = '';
    this.error = '';
    this.gameService.buyItem(itemId).subscribe({
      next: () => {
        this.message = 'Kupiono przedmiot!';
        setTimeout(() => this.message = '', 2000);
      },
      error: (err) => {
        this.error = err.error || 'Nie udało się kupić przedmiotu.';
        setTimeout(() => this.error = '', 3000);
      }
    });
  }
}
