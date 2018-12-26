import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { Post, Request } from '../../model';
import { ConfigService } from '../../config';
import { PostReactionService } from '../../services';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent {
  private _internalPost: Post;

  constructor(
    private configService: ConfigService,
    private postReactionService: PostReactionService) {
  }

  public get pictureUrl(): string {
    return this.configService.config.baseUrl + `posts/${this._internalPost.id}/picture`;
  }

  @Input()
  public set post(value: Post) {
    this._internalPost = value;
  }

  @Input()
  public set index(value: number) {
  }

  public get post(): Post {
    return this._internalPost;
  }

  public addReaction(state: number): void {
    this.postReactionService.addReaction(this._internalPost.id, state).then(reactionState => this._internalPost.reactions = reactionState);
  }
}
