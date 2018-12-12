import { Pipe, PipeTransform } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Pipe({ name: 'secure' })
export class SecurePipe implements PipeTransform {

    constructor(private readonly http: HttpClient) {
    }

    public transform(url: string) {
        return new Observable<string>((observer) => {
            // https://stackoverflow.com/questions/46563607/angular-4-image-async-with-bearer-headers
            // This is a tiny blank image
            observer.next('data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==');

            // The next and error callbacks from the observer
            const { next, error } = observer;

            this.http.get(url, { responseType: 'blob' }).subscribe(response => {
                const reader = new FileReader();
                reader.readAsDataURL(response);
                reader.onloadend = function () {
                    observer.next(<string>reader.result);
                };
            });

            return { unsubscribe() { } };
        });
    }
}
