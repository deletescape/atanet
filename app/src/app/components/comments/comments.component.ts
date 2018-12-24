
import {interval as observableInterval,  Observable ,  Subscription } from 'rxjs';
import { Component, OnInit, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { Post } from '../../model/post.model';

import { Comment } from '../../model/comment.model';
import { MatExpansionPanel } from '@angular/material';
import { SnackbarService } from '../../services';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsComponent implements OnInit {
  private _post: Post = undefined;

  constructor() {
  }

  @Input('post')
  public set post(value: Post) {
    this._post = value;
  }

  public ngOnInit(): void {
  }
}
