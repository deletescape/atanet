import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { Post, CreatedResult } from '../model';

@Injectable()
export class PostReactionService {

  constructor(private httpService: AtanetHttpService) { }

  public async addReaction(postId: number, state: number): Promise<CreatedResult> {
    const uri = `posts/${postId}/reactions`;
    const body = {
        ReactionState: state
    }
    const result = await this.httpService.post(uri, body, CreatedResult);
    return result;
  }
}
