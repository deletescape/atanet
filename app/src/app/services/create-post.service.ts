import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { CreatePost } from '../model';
import { CreatedResult } from '../model/created-result.model';

@Injectable()
export class CreatePostService {
  constructor(private httpService: AtanetHttpService) {
  }

  public async createPost(createPost: CreatePost): Promise<CreatedResult> {
    const uri = 'posts';
    const formData = new FormData();
    formData.append('Picture', createPost.file);
    formData.append('Text', createPost.text);
    const result = await this.httpService.post(uri, formData, CreatedResult);
    return result;
  }
}
