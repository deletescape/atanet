import { Pipe, PipeTransform } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Pipe({ name: 'secure' })
export class SecurePipe implements PipeTransform {

    private static readonly cache = {};

    constructor(private readonly http: HttpClient) {
    }

    public transform(url: string) {
        if (url in SecurePipe.cache) {
            return Observable.of(SecurePipe.cache[url]);
        }

        const result = this.http.get(url, { responseType: 'blob' })
            .switchMap(blob => {
                return Observable.create((observer: { next: (arg0: string | ArrayBuffer) => void; }) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(blob);
                    reader.onloadend = function () {
                        SecurePipe.cache[url] = reader.result;
                        observer.next(reader.result);
                    }
                });
            });
        return result;
    }
}
