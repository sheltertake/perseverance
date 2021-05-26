import { Component, HostListener, Pipe, PipeTransform } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { SignalRService } from './services/signal-r.service';
import { SimpleStateService } from './services/simple-state.service';

@Component({
  selector: 'app-root',
  template: `
    <!-- <pre>{{state$ | async | json}}</pre> -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Rover - Press arrows</a>
        <form class="d-flex">
          <span class="input-group-text">W</span>
          <input class="form-control me-1" type="number" name="W" [(ngModel)]="options.W" (ngModelChange)="restart()">
          <span class="input-group-text">H</span>
          <input class="form-control me-1" type="number" name="H" [(ngModel)]="options.H" (ngModelChange)="restart()">
          <span class="input-group-text">X</span>
          <input class="form-control me-1" type="number" name="X" [(ngModel)]="options.X" (ngModelChange)="restart()">
          <span class="input-group-text">Y</span>
          <input class="form-control me-1" type="number" name="Y" [(ngModel)]="options.Y" (ngModelChange)="restart()">
          <span class="input-group-text">O</span>
          <input class="form-control me-1" type="number" name="O" [(ngModel)]="options.O" (ngModelChange)="restart()">
        </form>
      </div>
    </nav>
    <main>
      <div class="flex-container" *ngFor="let row of (state$ | async)?.map;">
        <div class="flex-item" *ngFor="let cell of row;" [style.background-color]="cell | trisColor">
        </div>
      </div>
    </main>
  `,
  styles: [`
    .navbar { position:fixed; width:100%; }
    main{ height: 100vh;  }
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
        border:1px dashed #fff;
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
  options: Options = {
    H: 5,
    W: 5,
    X: 0,
    Y: 0,
    O: 5
  };

  constructor(
    private signalRService: SignalRService,
    private simpleStateService: SimpleStateService,
  ) { }

  ngOnInit() {
    this.signalRService.startConnection(this.options);
    this.signalRService.registerOnServerEvents();

    this.state$ = this.simpleStateService.state$;
    this.sub = this.simpleStateService.state$.subscribe(x => {
      this.state = x;
      console.log('isInError', this.state.isInError);
      if (this.state.isInError) {
        setTimeout(() => {
          (window as any).ion.sound.play("metal_plate");
        }, 100);
      }
    });


    (window as any).ion.sound({
      sounds: [
        {
          name: "metal_plate",
          multiplay: false,
          preload: true
        }
      ],
      volume: 0.5,
      path: "assets/sounds/"
    });

  }

  ngOnDestroy(): void {
    if (this.sub) {
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

  restart() {
    this.signalRService.LandRequestAsync(this.options);
  }

  toCommand(event: KeyboardEvent): string | undefined {
    switch (event.key) {
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

@Pipe({ name: 'trisColor' })
export class TrisColorPipe implements PipeTransform {
  transform(value: boolean | null): string {
    return value === true ? "black" :
      value === false ? "red" : "white";
  }
}


enum KEY_CODE {
  ARROW_RIGHT = 'ArrowRight',
  ARROW_LEFT = 'ArrowLeft',
  ARROW_UP = 'ArrowUp',
  ARROW_DOWN = 'ArrowDown',
}
export interface IState {
  map: boolean[][],
  guid: string,
  isInError: boolean
}
export interface Options {
  H: number,
  W: number,
  X: number,
  Y: number,
  O: number,
}