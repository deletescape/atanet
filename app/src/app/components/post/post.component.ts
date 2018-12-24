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
  private _hasVoted = false;
  private _address: string;
  private _index: number;

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

  public async addReaction(state: number): Promise<void> {
    const creationResult = await this.postReactionService.addReaction(this._internalPost.id, state);
  }
}
