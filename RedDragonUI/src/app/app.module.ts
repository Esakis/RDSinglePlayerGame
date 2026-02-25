import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { StartComponent } from './components/start/start.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BattleComponent } from './components/battle/battle.component';
import { ShopComponent } from './components/shop/shop.component';
import { InventoryComponent } from './components/inventory/inventory.component';
import { RankingComponent } from './components/ranking/ranking.component';

const routes: Routes = [
  { path: '', component: StartComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'battle', component: BattleComponent },
  { path: 'shop', component: ShopComponent },
  { path: 'inventory', component: InventoryComponent },
  { path: 'ranking', component: RankingComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    StartComponent,
    DashboardComponent,
    BattleComponent,
    ShopComponent,
    InventoryComponent,
    RankingComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
