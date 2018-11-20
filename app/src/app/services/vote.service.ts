import { Injectable, OnInit } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { CreatedResult } from '../model/created-result.model';
import * as moment from 'moment';

@Injectable()
export class VoteService {
  private readonly downVote = -1;
  private readonly upVote = 1;
  private votedPosts: number[] = new Array<number>();

  constructor(private httpService: AtanetHttpService) {
  }

  public async upvote(postId: number): Promise<number> {
    return await this.vote(this.upVote, postId);
  }

  public async downvote(postId: number): Promise<number> {
    return await this.vote(this.downVote, postId);
  }

  public hasVoted(postId: number): boolean {
    return this.votedPosts.indexOf(postId) > -1;
  }

  private async vote(value: number, postId: number): Promise<number> {
    const url = `Posts/${postId}/Votes?state=${value}`;
    const result = await this.httpService.post(url, {}, CreatedResult);
    this.votedPosts.push(postId);
    return result.createdId;
  }
}
