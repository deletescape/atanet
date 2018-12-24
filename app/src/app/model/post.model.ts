import { Comment } from './comment.model';
import { Reaction } from './reaction.model';

export class Post {
  public id: number | undefined = undefined;
  public text: string | undefined = undefined;
  public score: number | undefined = undefined;
  public created: Date | undefined = undefined;
  public comments: Array<Comment> | undefined = undefined;
  public reactions: Reaction | undefined = undefined;
}
