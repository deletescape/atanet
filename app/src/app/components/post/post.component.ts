import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { Post } from '../../model';
import {
  SnackbarService,
  AtanetHttpService
} from '../../services';

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
    private httpService: AtanetHttpService,
    private snackbarService: SnackbarService) {
  }

  @Input()
  public set post(value: Post) {
    this._internalPost = value;
  }

  @Input()
  public set index(value: number) {
    this._index = value;
  }

  public get post(): Post {
    return this._internalPost;
  }
}
