import { Pipe, PipeTransform } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Pipe({ name: 'secure' })
export class SecurePipe implements PipeTransform {

    constructor(private readonly http: HttpClient) {
    }

    public transform(url: string) {
        return this.http.get(url, { responseType: 'blob' })
            .switchMap(blob => {
                return Observable.create((observer: { next: (arg0: string | ArrayBuffer) => void; }) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(blob);
                    reader.onloadend = function () {
                        observer.next(reader.result);
                    }
                });
            });
    }
}
