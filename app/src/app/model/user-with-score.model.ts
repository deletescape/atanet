import { User } from "./user.model";

export class UserWithScore extends User {
    public created: Date | undefined = undefined;
    public score: number | undefined = undefined;
}
