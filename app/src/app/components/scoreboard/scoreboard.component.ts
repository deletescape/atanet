import { Component, OnInit } from '@angular/core';
import { UserHttpService } from '../../services';
import { UserWithScore } from '../../model/user-with-score.model';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.css']
})
export class ScoreboardComponent implements OnInit {

  constructor(private userService: UserHttpService) { }

  public users: UserWithScore[] = [];

  public ngOnInit(): void {
    this.load();
    setInterval(() => this.load(), 60000);
  }

  public load(): void {
    this.userService.getScoreboard().then(users => {
      this.users = users;
    });
  }

}
