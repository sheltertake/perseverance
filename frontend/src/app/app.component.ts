import { Component, HostListener, Injectable, Pipe, PipeTransform } from '@angular/core';
import { Observable, of, Subscription } from 'rxjs';
import { SignalRService } from './services/signal-r.service';
import { SimpleStateService } from './services/simple-state.service';

enum KEY_CODE {
  ARROW_RIGHT = 'ArrowRight',
  ARROW_LEFT = 'ArrowLeft',
  ARROW_UP = 'ArrowUp',
  ARROW_DOWN = 'ArrowDown',
}
export interface IState
{
  map: boolean[][],
  guid: string
}
// public class PerseveranceState
//     {
//         public byte X { get; init; }
//         public byte Y { get; init; }
//         public bool?[,] Map { get; init; }
//         public byte W { get; init; }
//         public byte H { get; init; }
//         public ICollection<Obstacle> Obstacles { get; set; }
//         public Guid Guid { get; internal set; }
//     }
@Component({
  selector: 'app-root',
  template: `
    <!-- <pre>{{state$ | async | json}}</pre> -->
    <div class="container">
      <div class="row" *ngFor="let row of (state$ | async)?.map">
        <div class="square" *ngFor="let cell of row">{{cell | tris}}</div>
      </div>
    </div>    
  `,
  styles: [`
    :host{height:100vh}
  `]
})
export class AppComponent {

  // map$: Observable<any> | undefined;
  // map2$: Observable<any> | undefined;
  state$!: Observable<IState>;
  state!: IState;
  sub!: Subscription;

  constructor(
    private signalRService: SignalRService,
    // private simpleService: SimpleService,
    private simpleStateService: SimpleStateService,
  ) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.registerOnServerEvents();

    // this.map$ = this.simpleService.Get();
    this.state$ = this.simpleStateService.state$;
    this.sub = this.simpleStateService.state$.subscribe(x =>{
      this.state = x;
    });
  }

  ngOnDestroy(): void {
    if(this.sub){
      this.sub.unsubscribe();
    }
  }
  
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    console.log(event, String.fromCharCode(event.keyCode));

    if (event.key === KEY_CODE.ARROW_RIGHT ||
      event.key === KEY_CODE.ARROW_LEFT ||
      event.key === KEY_CODE.ARROW_UP ||
      event.key === KEY_CODE.ARROW_DOWN) {
      // this.map$ = this.simpleService.Get(true);
      this.signalRService.MoveRequestAsync(this.state.guid, event.key);
    }
  }
}
@Pipe({ name: 'tris' })
export class TrisPipe implements PipeTransform {
  transform(value: boolean | null): string {
    return value === true ? "O" :
      value === false ? "X" : " ";
  }
}
@Injectable({
  providedIn: 'root'
})
export class SimpleService {

  Get(testRight: boolean = false): Observable<any> {
    const map = [
      [false, null, false],
      [null, true, null],
      [false, null, false]
    ];

    if(testRight){
      console.log('testRight', map);
      map[1][1]=null;
      map[1][2]=true;
      console.log(map)
    }

    return of(map);
  }
}