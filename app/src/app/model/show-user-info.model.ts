import { AtanetAction } from './actions.model';

export class ShowUserInfo {
    public id: number | undefined = undefined;
    public email: string | undefined = undefined;
    public score: number | undefined = undefined;
    public created: Date | undefined = undefined;
    public capabilities: AtanetAction[] | undefined = undefined;
}
