import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { IState } from "../app.component";

@Injectable({
    providedIn: 'root'
})
export class SimpleStateService {
    private stateSource$: Subject<IState>;
    public state$: Observable<IState>;

    constructor() {
        this.stateSource$ = new Subject<IState>();
        this.state$ = this.stateSource$.asObservable();
    }

    update(state: IState) {
        this.stateSource$.next(state);
    }
}