import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class EventsService {

    public refresh: EventEmitter<boolean> = new EventEmitter();

    public triggerRefresh(): void {
        this.refresh.emit();
    }
}
