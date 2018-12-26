import { Pipe, PipeTransform } from '@angular/core';
import { AtanetAction } from '../model';

@Pipe({ name: 'actionToString' })
export class AtanetActionPipe implements PipeTransform {
    public transform(value: any): string {
        switch (value) {
            case AtanetAction.DeleteLowScoreUser:
                return 'Delete users with low scores';
            case AtanetAction.CreateComment:
                return 'Comment on posts';
            case AtanetAction.ViewUserProfile:
                return 'View other users profiles';
            case AtanetAction.CreatePost:
                return 'Create new posts';
            case AtanetAction.VotePost:
                return 'Vote on posts';
            case AtanetAction.ViewOwnUserProfile:
                return 'View own user profile';
            default:
                return 'Do something';
        }
    }
}
