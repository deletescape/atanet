import { Component, Input, OnInit } from '@angular/core';
import { User } from '../../model/user.model';
import { ConfigService } from '../../config';
import { UserHttpService } from '../../services';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss']
})
export class UserItemComponent implements OnInit {

  private _userInfo: User;
  private _score: number = 0;
  private _joined: Date;

  constructor(private configService: ConfigService, private userHttpService: UserHttpService) { }

  @Input()
  public set user(userInfo: User) {
    this._userInfo = userInfo;
  }

  public get userId(): number {
    return this._userInfo.id;
  }

  public get userPictureUrl(): string {
    return this.configService.config.baseUrl + `users/picture?id=${this._userInfo.id}`;
  }

  public get userEmail(): string {
    return this._userInfo.email;
  }

  public get userScore(): number {
    return this._score;
  }

  public get joined(): Date {
    return this._joined;
  }

  public ngOnInit(): void {
    this.userHttpService.getUserInfo(this._userInfo.id).then(user => {
      this._score = user.score;
      this._joined = user.created;
    });
  }

}
