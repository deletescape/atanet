import { Injectable, Injector } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';
import { Score } from '../model/score.model';
import { ShowUserInfo } from '../model/show-user-info.model';
import { Request } from '../model';
import { UserWithScore } from '../model/user-with-score.model';

@Injectable()
export class UserHttpService {

  constructor(private readonly httpService: AtanetHttpService) {
  }

  public async getCurrentUserInfo(): Promise<ShowUserInfo> {
    const uri = 'users';
    return await this.httpService.get(uri, ShowUserInfo);
  }

  public async getUserInfo(userId?: number): Promise<ShowUserInfo> {
    const uri = `users/${userId || ''}`;
    const result = await this.httpService.get(uri, ShowUserInfo);
    return result;
  }

  public async deleteUser(userId: number): Promise<Request> {
    const uri = `users/${userId}`;
    return await this.httpService.delete(uri, Request);
  }

  public async getScore(): Promise<Score> {
    const uri = 'users/score';
    const result = await this.httpService.get(uri, Score);
    return result;
  }

  public async getScoreboard(): Promise<UserWithScore[]> {
    const uri = 'users/scoreboard';
    return await this.httpService.getArray(uri, UserWithScore);
  }
}
