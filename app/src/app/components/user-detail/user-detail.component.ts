import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserHttpService, SnackbarService } from '../../services';
import { ShowUserInfo } from '../../model/show-user-info.model';
import { MinScore, AtanetAction } from '../../model';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute,
              private userHttpService: UserHttpService,
              private router: Router,
              private snackbarService: SnackbarService) { }

  public userInfo: ShowUserInfo;
  public buttonDisabled: boolean = false;

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      const userId = <number>params['id'];
      this.userHttpService.getCurrentUserInfo().then(currentUser => {
        if (currentUser.score < MinScore[AtanetAction.ViewOwnUserProfile] && currentUser.id == userId) {
          this.snackbarService.showMessage('You cannot view your own user profile');
          this.router.navigate(['']);
          return;
        } else if (currentUser.score < MinScore[AtanetAction.ViewUserProfile] && currentUser.id != userId) {
          this.snackbarService.showMessage('You cannot view any user profiles');
          this.router.navigate(['']);
          return;
        }
      });
      this.userHttpService.getUserInfo(userId).then(userInfo => {
        this.userInfo = userInfo;
      });
    });
  }

  public delete(): void {
    this.buttonDisabled = true;
    this.userHttpService.deleteUser(this.userInfo.id).then(_ => {
      this.buttonDisabled = false;
      this.router.navigate(['']);
    }).catch(_ => {
      this.buttonDisabled = false;
      this.snackbarService.showMessage('Failed to delete user');
    });
  }

}
