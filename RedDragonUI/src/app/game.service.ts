import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

export interface CharacterDto {
  id: number;
  name: string;
  race: string;
  level: number;
  experience: number;
  experienceToNextLevel: number;
  maxHp: number;
  currentHp: number;
  maxMana: number;
  currentMana: number;
  strength: number;
  dexterity: number;
  intelligence: number;
  endurance: number;
  luck: number;
  statPoints: number;
  gold: number;
  weapon: ItemDto | null;
  armor: ItemDto | null;
}

export interface ItemDto {
  id: number;
  name: string;
  description: string;
  type: string;
  value: number;
  buyPrice: number;
  sellPrice: number;
  bonusStrength: number;
  bonusDexterity: number;
  bonusIntelligence: number;
  bonusEndurance: number;
  bonusHp: number;
  bonusMana: number;
  minDamage: number;
  maxDamage: number;
  defense: number;
  requiredLevel: number;
}

export interface InventoryItemDto {
  characterItemId: number;
  item: ItemDto;
  quantity: number;
}

export interface MonsterDto {
  id: number;
  name: string;
  description: string;
  level: number;
  maxHp: number;
  experienceReward: number;
  goldRewardMin: number;
  goldRewardMax: number;
}

export interface BattleResultDto {
  victory: boolean;
  experienceGained: number;
  goldGained: number;
  damageDealt: number;
  damageTaken: number;
  battleText: string;
  leveledUp: boolean;
  character: CharacterDto;
}

export interface BattleLogDto {
  id: number;
  monsterName: string;
  victory: boolean;
  experienceGained: number;
  goldGained: number;
  damageDealt: number;
  damageTaken: number;
  battleText: string;
  foughtAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private apiUrl = 'http://localhost:5069/api';
  private charId = 0;
  private characterSubject = new BehaviorSubject<CharacterDto | null>(null);
  character$ = this.characterSubject.asObservable();

  constructor(private http: HttpClient) {
    const savedCharId = localStorage.getItem('charId');
    if (savedCharId) {
      this.charId = parseInt(savedCharId, 10);
      this.refreshCharacter().subscribe();
    }
  }

  get currentCharacter(): CharacterDto | null {
    return this.characterSubject.value;
  }

  private setCharacter(c: CharacterDto): void {
    this.charId = c.id;
    localStorage.setItem('charId', c.id.toString());
    this.characterSubject.next(c);
  }

  register(name: string, password: string): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/auth/register`, { name, password })
      .pipe(tap(c => this.setCharacter(c)));
  }

  login(name: string, password: string): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/auth/login`, { name, password })
      .pipe(tap(c => this.setCharacter(c)));
  }

  loginAsGuest(): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/auth/guest`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  logout(): void {
    this.charId = 0;
    localStorage.removeItem('charId');
    this.characterSubject.next(null);
  }

  refreshCharacter(): Observable<CharacterDto> {
    return this.http.get<CharacterDto>(`${this.apiUrl}/character/${this.charId}`)
      .pipe(tap(c => this.setCharacter(c)));
  }

  allocateStat(stat: string, points: number): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/character/${this.charId}/allocate-stat`, { stat, points })
      .pipe(tap(c => this.setCharacter(c)));
  }

  rest(): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/character/${this.charId}/rest`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  getInventory(): Observable<InventoryItemDto[]> {
    return this.http.get<InventoryItemDto[]>(`${this.apiUrl}/character/${this.charId}/inventory`);
  }

  equipItem(itemId: number): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/character/${this.charId}/equip/${itemId}`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  usePotion(itemId: number): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/character/${this.charId}/use-potion/${itemId}`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  getMonsters(): Observable<MonsterDto[]> {
    return this.http.get<MonsterDto[]>(`${this.apiUrl}/battle/monsters`);
  }

  fight(monsterId: number): Observable<BattleResultDto> {
    return this.http.post<BattleResultDto>(`${this.apiUrl}/battle/${this.charId}/fight/${monsterId}`, {})
      .pipe(tap(r => this.setCharacter(r.character)));
  }

  getBattleLog(): Observable<BattleLogDto[]> {
    return this.http.get<BattleLogDto[]>(`${this.apiUrl}/battle/${this.charId}/log`);
  }

  getShopItems(): Observable<ItemDto[]> {
    return this.http.get<ItemDto[]>(`${this.apiUrl}/shop/items`);
  }

  buyItem(itemId: number): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/shop/${this.charId}/buy/${itemId}`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  sellItem(characterItemId: number): Observable<CharacterDto> {
    return this.http.post<CharacterDto>(`${this.apiUrl}/shop/${this.charId}/sell/${characterItemId}`, {})
      .pipe(tap(c => this.setCharacter(c)));
  }

  getRanking(): Observable<CharacterDto[]> {
    return this.http.get<CharacterDto[]>(`${this.apiUrl}/character/ranking`);
  }
}
