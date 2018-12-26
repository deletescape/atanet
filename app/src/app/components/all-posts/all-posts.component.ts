import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService, EventsService } from '../../services';
import { PostContainerRequest } from '../../model';

@Component({
  selector: 'app-all-posts',
  templateUrl: './all-posts.component.html',
  styleUrls: ['./all-posts.component.scss']
})
export class AllPostsComponent implements OnInit {
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') public postContainer: PostContainerComponent;

  constructor(private filterPostService: FilterPostService, private eventsService: EventsService) {
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

  public ngOnInit(): void {
    this.eventsService.refresh.subscribe(() => {
      this.refresh();
    });
  }

  public createRequest(): PostContainerRequest {
    return new PostContainerRequest(
      (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => {
        return request.filterPostService.getPosts(pageSize, page, commentAmount);
      }, {
      },
      this.filterPostService);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
