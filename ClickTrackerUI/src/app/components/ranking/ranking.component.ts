import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameService, CharacterDto } from '../../game.service';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit {
  rankings: CharacterDto[] = [];
  loading = true;

  constructor(private gameService: GameService, private router: Router) {}

  ngOnInit(): void {
    this.gameService.getRanking().subscribe({
      next: (data) => {
        this.rankings = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }
}
