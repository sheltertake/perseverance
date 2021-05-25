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

      <div class="flex-container" *ngFor="let row of (state$ | async)?.map;">
        <div class="flex-item" *ngFor="let cell of row;" [style.background-color]="cell | trisColor">
        </div>
      </div>

  `,
  styles: [`
    /* https://stackoverflow.com/questions/29307971/css-grid-of-squares-with-flexbox */
    .flex-container {
        padding: 0;
        margin: 0;
        list-style: none;
        display: flex;
        flex-flow: row;
        justify-content: space-around;
     
        line-height:30px;
    }
    .flex-item {
        background: tomato;
        color: white;
        flex: 1 0 auto;
        height:auto;
    }
    .flex-item:before {
        content:'';
        float:left;
        padding-top:100%;
    }
  `]
})
export class AppComponent {

  state$!: Observable<IState>;
  state!: IState;
  sub!: Subscription;

  constructor(
    private signalRService: SignalRService,
    private simpleStateService: SimpleStateService,
  ) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.registerOnServerEvents();

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
    const command = this.toCommand(event);
    if (command) {
      event.stopPropagation();
      this.signalRService.MoveRequestAsync(this.state.guid, command);
    }
  }

  toCommand(event: KeyboardEvent) : string | undefined {
    switch(event.key){
      case KEY_CODE.ARROW_RIGHT:
        return 'R';
        
      case KEY_CODE.ARROW_LEFT:
        return 'L';
        
      case KEY_CODE.ARROW_UP:
        return 'B';
        
      case KEY_CODE.ARROW_DOWN:
        return 'F';
    }
    return undefined;
  }
}
@Pipe({ name: 'tris' })
export class TrisPipe implements PipeTransform {
  transform(value: boolean | null): string {
    return value === true ? "O" :
      value === false ? "X" : "";
  }
}
@Pipe({ name: 'trisColor' })
export class TrisColorPipe implements PipeTransform {
  transform(value: boolean | null): string {
    return value === true ? "black" :
      value === false ? "red" : "white";
  }
}