import { Component, OnInit } from '@angular/core';
import { UserHttpService } from '../../services';
import { User } from '../../model/user.model';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.css']
})
export class ScoreboardComponent implements OnInit {

  constructor(private userService: UserHttpService) { }

  public users: User[] = [];

  public ngOnInit(): void {
    this.load();
    setInterval(() => this.load(), 10000);
  }

  public load(): void {
    this.userService.getScoreboard().then(users => {
      this.users = users;
    });
  }

}
