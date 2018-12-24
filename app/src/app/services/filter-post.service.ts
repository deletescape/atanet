import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { Post } from '../model';

@Injectable()
export class FilterPostService {

  constructor(private httpService: AtanetHttpService) { }

  public async getPosts(pageSize: number, page?: number, comments?: number): Promise<Post[]> {
    let uri = `posts?pageSize=${pageSize}`;
    if (page) {
      uri += `&page=${page}`;
    }

    if (comments) {
      uri += `&comments=${page}`;
    }

    const result = await this.httpService.getArray<Post>(uri, Post);
    return result;
  }
}
