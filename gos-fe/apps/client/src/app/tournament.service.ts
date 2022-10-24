import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TournamentService {
  public tournamentResult?: any;

  private baseUrl = `http://localhost:4200/api/`;

  constructor(private httpClient: HttpClient) {}

  public runTournament() {
    this.httpClient.post(this.baseUrl + 'tournament', {}).subscribe(
      (res) => {
        console.log(res);
        this.tournamentResult = res;
      },
      (error) => console.error(error)
    );
  }
}
