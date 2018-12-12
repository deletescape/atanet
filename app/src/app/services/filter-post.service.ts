import { Injectable } from '@angular/core';
import { AtanetHttpService } from './atanet-http.service';

@Injectable()
export class FilterPostService {

  constructor(private httpService: AtanetHttpService) { }

}
