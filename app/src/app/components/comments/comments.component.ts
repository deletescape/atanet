
import { Component, ViewEncapsulation, Input, AfterViewInit } from '@angular/core';
import { Post } from '../../model/post.model';
import { Comment } from '../../model/comment.model';
import { CommentHttpService } from '../../services';
import { ConfigService } from '../../config';
import { User } from '../../model/user.model';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsComponent implements AfterViewInit {
  private _post: Post = undefined;
  private readonly _id: string;

  constructor(public domSanitizer: DomSanitizer, private commentHttpService: CommentHttpService, private configService: ConfigService) {
    this._id = Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 36);
  }

  public text: string = '';
  private _currentPage: number = 1;
  private readonly _pageSize: number = 3;

  @Input('post')
  public set post(value: Post) {
    this._post = value;
  }

  public get id(): string {
    return this._id;
  }

  public get comments(): Comment[] {
    return this._post.comments;
  }

  public addComment(): void {
    this.commentHttpService.addComment(this._post.id, this.text).then(_ => {
      this.text = '';
      this.refresh();
    });
  }

  public refresh(): void {
    this._post.comments = [];
    this._currentPage = 0;
    this.loadPage();
  }

  public ngAfterViewInit(): void {
    // @ts-ignore
    mediumZoom('#' + this.id, {
    });
  }

  public getPicture(id: number): string {
    return this.configService.config.baseUrl + `users/picture?id=${id}`;
  }

  public loadPage(): void {
    this.commentHttpService.filterComments(this._post.id, this._currentPage, this._pageSize).then(comments => {
      if (this._currentPage === 0) {
        this._post.comments = [];
      }
      
      for (const comment of comments) {
        this._post.comments.push(comment);
      }

      if (this.comments.length < this._pageSize) {
        return;
      }

      this._currentPage++;
    });
  }
}
