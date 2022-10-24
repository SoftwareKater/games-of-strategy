import { Component } from '@angular/core';
import { TournamentService } from './tournament.service';

@Component({
  selector: 'gos-fe-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  public result?: any = this.tournamentService.tournamentResult;

  constructor(private readonly tournamentService: TournamentService) {}

  public runTournament() {
    this.tournamentService.runTournament();
  }
}
