import { Component, OnInit } from '@angular/core';
import { ClickService } from './click.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Click Tracker';
  clickCount: number = 0;
  loading: boolean = false;

  constructor(private clickService: ClickService) { }

  ngOnInit(): void {
    this.loadClickCount();
  }

  loadClickCount(): void {
    this.clickService.getClickCount().subscribe({
      next: (count) => {
        this.clickCount = count;
      },
      error: (error) => {
        console.error('Error loading click count:', error);
      }
    });
  }

  onButtonClick(): void {
    this.loading = true;
    this.clickService.recordClick().subscribe({
      next: (response) => {
        this.clickCount++;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error recording click:', error);
        this.loading = false;
      }
    });
  }
}
