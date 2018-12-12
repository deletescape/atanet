import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import { AuthService } from 'angular-6-social-login';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(private readonly auth: AuthService) { }

    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // TODO: early return with token from localStorage
        return this.auth.authState.mergeMap(socialUser => {
            if (!socialUser) {
                return Observable.empty().toPromise();
            }

            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${socialUser.idToken}`
                }
            });
            return next.handle(request);
        });

    }
}