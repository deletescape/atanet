export class CreatePost {
    constructor(text: string, file: any) {
        this.text = text;
        this.file = file;
    }

    public text: string | undefined = undefined;
    public file: any | undefined = undefined;
    
}
