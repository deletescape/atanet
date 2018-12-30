import { AtanetAction } from './actions.model';
import { Picture } from './picture.model';

export class ShowUserInfo {
    public id: number | undefined = undefined;
    public email: string | undefined = undefined;
    public score: number | undefined = undefined;
    public created: Date | undefined = undefined;
    public picture: Picture | undefined = undefined;
    public capabilities: AtanetAction[] | undefined = undefined;
}
