import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameService, MonsterDto, BattleResultDto } from '../../game.service';

@Component({
  selector: 'app-battle',
  templateUrl: './battle.component.html',
  styleUrls: ['./battle.component.css']
})
export class BattleComponent implements OnInit {
  monsters: MonsterDto[] = [];
  battleResult: BattleResultDto | null = null;
  fighting = false;
  error = '';

  constructor(private gameService: GameService, private router: Router) {}

  ngOnInit(): void {
    this.loadMonsters();
  }

  loadMonsters(): void {
    this.gameService.getMonsters().subscribe({
      next: (monsters) => this.monsters = monsters,
      error: () => this.error = 'Nie udało się załadować listy potworów.'
    });
  }

  fight(monsterId: number): void {
    this.fighting = true;
    this.battleResult = null;
    this.error = '';

    this.gameService.fight(monsterId).subscribe({
      next: (result) => {
        this.battleResult = result;
        this.fighting = false;
      },
      error: (err) => {
        this.error = err.error || 'Błąd podczas walki.';
        this.fighting = false;
      }
    });
  }

  closeBattle(): void {
    this.battleResult = null;
  }
}
