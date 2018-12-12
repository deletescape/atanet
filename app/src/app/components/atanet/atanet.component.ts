import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'angular-6-social-login';
import { Router } from '@angular/router';
import { AllPostsComponent } from '../all-posts';

@Component({
  selector: 'app-atanet',
  templateUrl: './atanet.component.html',
  styleUrls: ['./atanet.component.scss']
})
export class AtanetComponent implements OnInit {
  @ViewChild('all') public all: AllPostsComponent;
  public userEmail: string;

  constructor(private socialAuthService: AuthService, private router: Router) {
  }

  public ngOnInit(): void {
    this.all.isActive = true;
    this.socialAuthService.authState.subscribe(authState => {
      if (!authState || !authState.email) {
        this.router.navigate(['/login']);
        return;
      }

      this.userEmail = authState.email;
    });
  }

}
