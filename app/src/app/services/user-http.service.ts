import { Injectable, Injector } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { Score } from '../model/score.model';
import { User } from '../model';

@Injectable()
export class UserHttpService {

  constructor(private readonly httpService: AtanetHttpService) {
  }

  public async getScore(): Promise<Score> {
    const uri = 'users/score';
    const result = await this.httpService.get(uri, Score);
    return result;
  }

  public async getUserInfo(userId: number = undefined): Promise<User> {
    const uri = userId ? `users/${userId}` :  'users';
    const result = await this.httpService.get(uri, User);
    return result;
  }
}
