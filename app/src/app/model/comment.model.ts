import { User } from "./user.model";

export class Comment {
    public id: number | undefined = undefined;
    public text: string | undefined = undefined;
    public created: Date | undefined = undefined;
    public postId: number | undefined = undefined;
    public user: User | undefined = undefined;
}
