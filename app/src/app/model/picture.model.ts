import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

export class Picture {
    public base64Data: string | undefined = undefined;
    public contentType: string | undefined = undefined;

    public imageSource(domSanitizer: DomSanitizer): SafeUrl {
        return domSanitizer.bypassSecurityTrustUrl('data:' + this.contentType + ';base64, ' + this.base64Data);
    }
}
