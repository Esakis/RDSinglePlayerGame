import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ClickEvent {
  id: number;
  clickedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class ClickService {
  private apiUrl = 'http://localhost:5069/api/clicks';

  constructor(private http: HttpClient) { }

  getClickCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count`);
  }

  recordClick(): Observable<ClickEvent> {
    return this.http.post<ClickEvent>(this.apiUrl, {});
  }

  getAllClicks(): Observable<ClickEvent[]> {
    return this.http.get<ClickEvent[]>(this.apiUrl);
  }
}
