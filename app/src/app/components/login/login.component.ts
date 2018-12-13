import { Component, OnInit } from '@angular/core';
import { AuthService, GoogleLoginProvider } from 'angular-6-social-login';
import { Router } from '@angular/router';
import { ConfigService } from '../../config';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private config: ConfigService) { }

  public email: string;

  public url: string = '';

  public ngOnInit(): void {
    this.url = this.config.config.baseUrl + 'files/expression';
    this.authService.authState.subscribe(authState => {
      if (authState) {
        this.router.navigate(['/']);
      }
    });
  }

  public login(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(user => {
      if (user && user.email) {
        this.email = user.email;
        this.router.navigate(['']);
      }
    }).catch(err => {
      console.warn(err);
    });
  }

}
