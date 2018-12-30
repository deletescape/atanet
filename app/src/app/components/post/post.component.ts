import { Component, Input, AfterViewInit } from '@angular/core';
import { Post } from '../../model';
import { ConfigService } from '../../config';
import { PostReactionService } from '../../services';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements AfterViewInit {
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

  public ngAfterViewInit() {
    // @ts-ignore
    mediumZoom('#post-image-' + this.post.id, {
    });
  }

  public addReaction(state: number): void {
    this.postReactionService.addReaction(this._internalPost.id, state).then(reactionState => this._internalPost.reactions = reactionState);
  }
}
