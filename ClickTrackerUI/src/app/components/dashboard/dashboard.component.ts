import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameService, CharacterDto } from '../../game.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  character: CharacterDto | null = null;
  resting = false;
  message = '';

  constructor(private gameService: GameService, private router: Router) {}

  ngOnInit(): void {
    this.gameService.character$.subscribe(c => this.character = c);
    if (this.gameService.currentCharacter) {
      this.gameService.refreshCharacter().subscribe();
    }
  }

  rest(): void {
    this.resting = true;
    this.gameService.rest().subscribe({
      next: () => {
        this.message = 'Odpoczywasz... HP i Mana w pełni przywrócone!';
        this.resting = false;
        setTimeout(() => this.message = '', 3000);
      },
      error: () => {
        this.message = 'Błąd podczas odpoczynku.';
        this.resting = false;
      }
    });
  }

  allocate(stat: string): void {
    this.gameService.allocateStat(stat, 1).subscribe({
      next: () => {
        this.message = `Przydzielono punkt do: ${stat}`;
        setTimeout(() => this.message = '', 2000);
      },
      error: (err) => {
        this.message = err.error || 'Błąd przydzielania punktów.';
      }
    });
  }

  getExpPercent(): number {
    if (!this.character) return 0;
    return (this.character.experience / this.character.experienceToNextLevel) * 100;
  }

  getHpPercent(): number {
    if (!this.character) return 0;
    return (this.character.currentHp / this.character.maxHp) * 100;
  }

  getManaPercent(): number {
    if (!this.character) return 0;
    return (this.character.currentMana / this.character.maxMana) * 100;
  }
}
