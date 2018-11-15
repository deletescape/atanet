export class File {
  public id: number | undefined = undefined;
  public fileName: string | undefined = undefined;
  public contentType: string | undefined = undefined;
  public created: Date | undefined = undefined;
  public link: string | undefined = undefined;

  public get url(): string {
    if (this.link) {
      return this.link;
    } else {
      return '/atanet/api/Files/' + this.id;
    }
  }
}
