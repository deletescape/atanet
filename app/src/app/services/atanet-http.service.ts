import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import { SnackbarService } from './snackbar.service';
import * as moment from 'moment';
import { BadRequest } from '../model';
import { ConfigService } from '../config';
import { AuthService } from 'angular-6-social-login';
import { Observable } from 'rxjs';
import { Reaction } from '../model/reaction.model';
import { User } from '../model/user.model';

declare type ConstructorType = { new(): any };

@Injectable()
export class AtanetHttpService {
  private readonly baseUri: string = '';
  private readonly propertyAdapters: { propertyName: string, constructor: ConstructorType, isArray: boolean }[] = [];
  private token: string;

  constructor(private httpClient: HttpClient, private snackbarService: SnackbarService, private configService: ConfigService, private authService: AuthService) {
    this.baseUri = this.configService.config.baseUrl;
    this.registerAdapters();
  }

  public get BaseUri(): string {
    return this.baseUri;
  }

  public async get<T>(uri: string, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const resultObject = await this.handleRequest(
      () => this.httpClient.get(combinedUri),
      type,
      false);
    return <T>resultObject;
  }

  public async getArray<T>(uri: string, type: { new(): T }): Promise<T[]> {
    const combinedUri = this.baseUri + uri;
    const resultObject = await this.handleRequest(
      () => this.httpClient.get(combinedUri),
      type,
      true);
    return <T[]>resultObject;
  }

  public async post<T>(uri: string, body: any, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const resultObject = await this.handleRequest(
      () => this.httpClient.post(combinedUri, body),
      type,
      false);
    return <T>resultObject;
  }

  public async handleRequest<T>(request: () => Observable<Object>, typeConstructor: { new(): T }, isArray: boolean): Promise<T | T[]> {
    return new Promise<T | T[]>((resolve, reject) => {
      request().subscribe(response => {
        if (isArray) {
          const array = <any[]>response;
          const resultArray = [];
          for (const i of array) {
            const parsedResult = this.copyProperties<T>(i, typeConstructor);
            resultArray.push(parsedResult);
          }

          resolve(resultArray);
        } else {
          const result = this.copyProperties<T>(response, typeConstructor);
          resolve(result);
        }
      }, error => {
        switch (error.status) {
          case 500:
            this.snackbarService.showMessage(error.message, 7000);
            break;
          case 400:
            const response = <BadRequest>error.error;
            this.snackbarService.showMessage(response.message);
            break;
          case 401:
            this.snackbarService.showMessage('You\'re not authorized to access this resource');
            break;
          case 403:
            this.snackbarService.showMessage('Forbidden');
            break;
        }
        reject(error);
      });
    });
  }

  public copyProperties<T>(source: any, target: { new(): T }): T {
    if (source === null || source === undefined) {
      return null;
    }

    const result = new target();
    for (const key of Object.keys(result)) {
      if (source.hasOwnProperty(key)) {
        if (this.adapterExists(key)) {
          const constructor = this.getConstructor(key);
          const isArray = this.getIsArray(key);
          if (isArray) {
            result[key] = [];
            for (const item of source[key]) {
              const created = this.copyProperties(item, constructor);
              result[key].push(created);
            }
          } else {
            const created = this.copyProperties(source[key], constructor);
            result[key] = created;
          }
          continue;
        }

        const date = moment(source[key]);
        if (date.isValid() && typeof source[key] === 'string' && source[key].indexOf(' ') < 0 && key !== 'text') {
          result[key] = date.toDate();
        } else {
          result[key] = source[key];
        }
      }
    }

    return result;
  }

  private adapterExists(propertyName: string): boolean {
    return this.propertyAdapters.map(x => x.propertyName).indexOf(propertyName) > -1;
  }

  private getConstructor(propertyName: string): ConstructorType {
    return this.propertyAdapters.find(x => x.propertyName === propertyName).constructor;
  }

  private getIsArray(propertyName: string): boolean {
    return this.propertyAdapters.find(x => x.propertyName === propertyName).isArray;
  }

  private registerAdapters(): void {
    this.propertyAdapters.push({
      propertyName: 'comments',
      constructor: Comment,
      isArray: true
    });
    this.propertyAdapters.push({
      propertyName: 'reactions',
      constructor: Reaction,
      isArray: false
    });
    this.propertyAdapters.push({
      propertyName: 'user',
      constructor: User,
      isArray: false
    });
  }
}
