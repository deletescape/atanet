import { Comment } from './comment.model';

export class Post {
  public id: number | undefined = undefined;
  public text: string | undefined = undefined;
  public topic: string | undefined = undefined;
  public voteCount: number | undefined = undefined;
  public created: Date | undefined = undefined;
  public comments: Array<Comment> | undefined = undefined;
}
