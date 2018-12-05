import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { GlobalErrorHandler } from './global-error-handler';
import {
  MatButtonModule,
  MatToolbarModule,
  MatTabsModule,
  MatCardModule,
  MatInputModule,
  MatDialogModule,
  MatSnackBarModule,
  MatProgressSpinnerModule,
  MatChipsModule,
  MatProgressBarModule,
  MatSelectModule,
  MatIconModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatFormFieldModule,
  MatButtonToggleModule,
  MatExpansionModule,
  MatGridListModule,
  MatListModule
} from '@angular/material';

import * as comp from './components';
import * as serv from './services';
import * as pipe from './pipes';
import { CommonModule } from '@angular/common';
import { ConfigService } from './config';
import { SocialLoginModule, AuthServiceConfig, GoogleLoginProvider } from 'angular-6-social-login';


export function getAuthServiceConfigs() {
  let config = new AuthServiceConfig(
      [
        {
          id: GoogleLoginProvider.PROVIDER_ID,
          provider: new GoogleLoginProvider('183413621231-odlcpmht4o9dnqj2v0rpgcm3a1h2dd0e.apps.googleusercontent.com')
        }
      ]);
  return config;
}


export function init(_boot: ConfigService) {
  return () => {
    return _boot.loadConfig();
  };
}

@NgModule({
  declarations: [
    comp.AppComponent,
    comp.CreatePostComponent,
    comp.PopularPostsComponent,
    comp.NewestPostsComponent,
    comp.TaggedPostsComponent,
    comp.PostContainerComponent,
    comp.PostComponent,
    pipe.PrefixNumberPipe,
    pipe.PostTagNamePipe,
    comp.QueryPostsComponent,
    comp.DatePostsComponent,
    comp.LocationPostsComponent,
    comp.CommentsComponent,
    comp.PostFileComponent,
    comp.FileDialogComponent,
    comp.CreateFileComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatTabsModule,
    MatCardModule,
    MatInputModule,
    FormsModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatProgressBarModule,
    MatSelectModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatButtonToggleModule,
    MatExpansionModule,
    MatGridListModule,
    MatListModule,
    SocialLoginModule
  ],
  providers: [
    ConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: init,
      deps: [ConfigService],
      multi: true
    },
    serv.CreatePostService,
    serv.AtanetHttpService,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    serv.SnackbarService,
    serv.VoteService,
    serv.FilterPostService,
    serv.TagService,
    serv.LocationService,
    serv.FilterCommentService,
    serv.CreateCommentService,
    serv.FileService,
    {
      provide: AuthServiceConfig,
      useFactory: getAuthServiceConfigs
    }
  ],
  entryComponents: [
    comp.CreatePostComponent,
    comp.FileDialogComponent,
    comp.CreateFileComponent
  ],
  bootstrap: [comp.AppComponent]
})
export class AppModule { }
