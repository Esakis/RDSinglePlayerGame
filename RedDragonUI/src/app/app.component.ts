import { Component } from '@angular/core';
import { GameService, CharacterDto } from './game.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  character: CharacterDto | null = null;

  constructor(public gameService: GameService) {
    this.gameService.character$.subscribe(c => this.character = c);
  }
}
