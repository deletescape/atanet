export class CreatePost {
    constructor(text: string) {
        this.text = text;
    }

    public text: string | undefined = undefined;
    public fileId: number | undefined = undefined;
}
