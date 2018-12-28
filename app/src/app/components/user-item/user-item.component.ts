import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { User } from '../../model/user.model';
import { ConfigService } from '../../config';
import { UserHttpService } from '../../services';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss']
})
export class UserItemComponent implements OnInit, AfterViewInit {

  private _userInfo: User;
  private _score: number = 0;
  private _joined: Date;
  private readonly _id: string;

  constructor(private configService: ConfigService, private userHttpService: UserHttpService) {
    this._id = Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 36);
  }

  @Input()
  public set user(userInfo: User) {
    this._userInfo = userInfo;
  }

  public get id(): string {
    return this._id;
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

  public ngAfterViewInit(): void {
    // @ts-ignore
    mediumZoom('#' + this.id, {
    });
  }

  public ngOnInit(): void {
    this.userHttpService.getUserInfo(this._userInfo.id).then(user => {
      this._score = user.score;
      this._joined = user.created;
    });
  }

}