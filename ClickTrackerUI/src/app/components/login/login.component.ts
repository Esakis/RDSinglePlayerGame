import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameService } from '../../click.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  name = '';
  password = '';
  isRegister = false;
  error = '';
  loading = false;

  constructor(private gameService: GameService, private router: Router) {
    if (this.gameService.currentCharacter) {
      this.router.navigate(['/dashboard']);
    }
  }

  submit(): void {
    this.error = '';
    this.loading = true;

    const action = this.isRegister
      ? this.gameService.register(this.name, this.password)
      : this.gameService.login(this.name, this.password);

    action.subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading = false;
        if (err.error && typeof err.error === 'string') {
          this.error = err.error;
        } else {
          this.error = this.isRegister ? 'Błąd rejestracji.' : 'Nieprawidłowa nazwa lub hasło.';
        }
      }
    });
  }
}
