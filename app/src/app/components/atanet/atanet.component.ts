import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AuthService } from 'angular-6-social-login';
import { NewestPostsComponent } from '../newest-posts';
import { CreatePostComponent } from '../create-post';
import { Router } from '@angular/router';

@Component({
  selector: 'app-atanet',
  templateUrl: './atanet.component.html',
  styleUrls: ['./atanet.component.css']
})
export class AtanetComponent implements OnInit {
  @ViewChild('newest') public newest: NewestPostsComponent;
  public isRefreshing = false;
  public userEmail: string;

  constructor(private dialog: MatDialog, private socialAuthService: AuthService, private router: Router) {
  }

  public ngOnInit(): void {
    this.newest.isActive = true;
    this.socialAuthService.authState.subscribe(authState => {
      if (!authState || !authState.email) {
        this.router.navigate(['login'])
      }

      this.userEmail = authState.email
    });
  }

  public showDialog(): void {2
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
