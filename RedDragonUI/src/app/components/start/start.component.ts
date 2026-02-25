import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameService } from '../../game.service';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css']
})
export class StartComponent {
  loading = false;
  error = '';

  constructor(private gameService: GameService, private router: Router) {}

  playAsGuest(): void {
    this.loading = true;
    this.error = '';
    this.gameService.refreshCharacter().subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => {
        this.loading = false;
        this.error = 'Nie udało się połączyć z serwerem. Upewnij się, że API działa.';
      }
    });
  }
}
