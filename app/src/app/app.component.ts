import { Component, OnInit } from '@angular/core';
import { AuthService, SocialUser } from 'angular-6-social-login';
import { Router } from '@angular/router';
import { CreatePostComponent } from './components';
import { MatDialog } from '@angular/material';
import { ConfigService } from './config';
import { UserHttpService, EventsService } from './services';
import { ShowUserInfo } from './model/show-user-info.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public authState: SocialUser;
  public userInfo: ShowUserInfo;
  public score: number = 0;

  constructor(private dialog: MatDialog,
              private authService: AuthService,
              private router: Router,
              private config: ConfigService,
              private userHttpService: UserHttpService,
              private eventsService: EventsService) {
  }

  public get pictureUrl(): string {
    return this.config.config.baseUrl + 'users/picture';
  }

  public logout(): void {
    const res = () => {
      this.router.navigate(['/login']);
    };
    this.authService.signOut().then(res).catch(res);
  }

  public goToUserProfile(): void {
    this.router.navigate(['user/' + this.userInfo.id]);
  }

  public createPost(): void {
    const dialogRef = this.dialog.open(CreatePostComponent, {
      width: '70%'
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result > 0) {
        await this.refresh();
      }
    });
  }

  public async refresh(): Promise<void> {
    this.eventsService.triggerRefresh();
  }

  public ngOnInit(): void {
    this.authService.authState.subscribe(authState => {
      this.authState = authState;
    });
    this.setScore();
    setInterval(() => {
      this.setScore();
    }, 5000);
  }

  private setScore(): void {
    this.userHttpService.getCurrentUserInfo().then(userInfo => this.userInfo = userInfo);
    this.userHttpService.getScore().then(score => this.score = score.score);
  }
}
