import { Injectable, Injector } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { Score } from '../model/score.model';

@Injectable()
export class UserHttpService {

  constructor(private readonly httpService: AtanetHttpService) {
  }

  public async getScore(): Promise<Score> {
    const uri = 'users/score';
    const result = await this.httpService.get(uri, Score);
    return result;
  }
}
