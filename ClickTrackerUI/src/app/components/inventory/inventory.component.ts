import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameService, InventoryItemDto, CharacterDto } from '../../game.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {
  inventory: InventoryItemDto[] = [];
  character: CharacterDto | null = null;
  message = '';
  error = '';

  constructor(private gameService: GameService, private router: Router) {}

  ngOnInit(): void {
    this.gameService.character$.subscribe(c => this.character = c);
    this.loadInventory();
  }

  loadInventory(): void {
    this.gameService.getInventory().subscribe({
      next: (items) => this.inventory = items,
      error: () => this.error = 'Nie udało się załadować ekwipunku.'
    });
  }

  equip(itemId: number): void {
    this.message = '';
    this.error = '';
    this.gameService.equipItem(itemId).subscribe({
      next: () => {
        this.message = 'Przedmiot założony!';
        setTimeout(() => this.message = '', 2000);
      },
      error: (err) => {
        this.error = err.error || 'Nie udało się założyć przedmiotu.';
        setTimeout(() => this.error = '', 3000);
      }
    });
  }

  usePotion(itemId: number): void {
    this.message = '';
    this.error = '';
    this.gameService.usePotion(itemId).subscribe({
      next: () => {
        this.message = 'Użyto mikstury!';
        this.loadInventory();
        setTimeout(() => this.message = '', 2000);
      },
      error: (err) => {
        this.error = err.error || 'Nie udało się użyć mikstury.';
        setTimeout(() => this.error = '', 3000);
      }
    });
  }

  sell(characterItemId: number): void {
    this.message = '';
    this.error = '';
    this.gameService.sellItem(characterItemId).subscribe({
      next: () => {
        this.message = 'Sprzedano przedmiot!';
        this.loadInventory();
        setTimeout(() => this.message = '', 2000);
      },
      error: (err) => {
        this.error = err.error || 'Nie udało się sprzedać przedmiotu.';
        setTimeout(() => this.error = '', 3000);
      }
    });
  }

  isEquipped(itemId: number, type: string): boolean {
    if (!this.character) return false;
    if (type === 'Weapon') return this.character.weapon?.id === itemId;
    if (type === 'Armor') return this.character.armor?.id === itemId;
    return false;
  }
}
