import { Component, OnInit } from '@angular/core';
import { User } from '../../model';
import { UserHttpService } from '../../services';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user = new User();
  score = 22;

  constructor(private userService: UserHttpService) {
    this.user.capabilities = [1, 5, 42];
    this.user.email = "me@notdeletescape.ch";
    this.user.created = new Date();
    userService.getUserInfo().then(function (user: User) {
      this.user = user;
    });
    userService.getScore().then(function (score) {
      this.score = score;
    });
  }

  ngOnInit() {
  }

}
