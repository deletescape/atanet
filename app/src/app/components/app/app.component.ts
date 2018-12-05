import { Component, ViewChild, OnInit } from '@angular/core';
import { MatDialog, MatTabGroup, MatToolbar } from '@angular/material';
import { CreatePostComponent } from '../create-post';
import { NewestPostsComponent } from '../newest-posts';
import { AuthService, GoogleLoginProvider } from 'angular-6-social-login';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  @ViewChild('newest') public newest: NewestPostsComponent;
  @ViewChild('tabGroup') public tabGroup: MatTabGroup;
  public isRefreshing = false;

  constructor(private dialog: MatDialog, private socialAuthService: AuthService) {
  }

  public ngOnInit(): void {
    this.newest.isActive = true;
    this.tabGroup.selectedIndexChange.subscribe(async _ => {
      await this.refresh();
    });
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID).then(userData => {
      console.log(userData);
    });
  }

  public showDialog(): void {
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
    await this.newest.refresh();
  }
}
