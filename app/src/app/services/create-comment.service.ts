import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { CreatedResult } from '../model/created-result.model';

@Injectable()
export class CreateCommentService {

  constructor(private httpService: AtanetHttpService) { }

  public async createComment(postId: number, text: string): Promise<number> {
    const url = `Posts/${postId}/Comments`;
    const result = await this.httpService.post(url, {
      text: text
    }, CreatedResult);
    return result.createdId;
  }
}
