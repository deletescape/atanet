import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { PostContainerRequest } from '../../model';

@Component({
  selector: 'app-all-posts',
  templateUrl: './all-posts.component.html',
  styleUrls: ['./all-posts.component.scss']
})
export class AllPostsComponent {
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') public postContainer: PostContainerComponent;

  constructor(private filterPostService: FilterPostService) {
    this._fetchRequest = this.createRequest();
  }

  public set isActive(value: boolean) {
    this._isActive = value;
  }

  public get isActive(): boolean {
    return this._isActive;
  }

  public get fetchRequest(): PostContainerRequest {
    return this._fetchRequest;
  }

  public createRequest(): PostContainerRequest {
    return new PostContainerRequest(
      (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => {
        return undefined;
        // return request.filterPostService.filter(page, pageSize, commentAmount);
      }, {
      },
      this.filterPostService);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
