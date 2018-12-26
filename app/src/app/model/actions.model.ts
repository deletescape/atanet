export enum AtanetAction {
    DeleteLowScoreUser = 0,
    CreateComment = 1,
    ViewUserProfile = 2,
    CreatePost = 3,
    VotePost = 4,
    ViewOwnUserProfile = 5
}

const MinScore = {};
MinScore[AtanetAction.DeleteLowScoreUser] = 200;
MinScore[AtanetAction.CreateComment] = 75;
MinScore[AtanetAction.ViewUserProfile] = 50;
MinScore[AtanetAction.CreatePost] = 0;
MinScore[AtanetAction.VotePost] = -10;
MinScore[AtanetAction.ViewOwnUserProfile] = -100;
export { MinScore };
