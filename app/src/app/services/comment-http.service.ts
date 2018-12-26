import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { CreatedResult } from '../model';
import { Comment } from '../model/comment.model';

@Injectable()
export class CommentHttpService {

  constructor(private httpService: AtanetHttpService) { }

  public async addComment(postId: number, comment: string): Promise<CreatedResult> {
    const uri = `posts/${postId}/comments`;
    const body = {
        text: comment
    }
    const result = await this.httpService.post(uri, body, CreatedResult);
    return result;
  }

  public async filterComments(postId: number, page: number, pageSize: number): Promise<Comment[]> {
    const uri = `posts/${postId}/comments?page=${page}&pageSize=${pageSize}`;
    return await this.httpService.getArray(uri, Comment);
  }

}
