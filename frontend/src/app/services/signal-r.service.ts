import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { IState, Options } from '../app.component';
import { SimpleStateService } from './simple-state.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor(private simpleStateService: SimpleStateService) {

  }
  private hubConnection!: signalR.HubConnection;
  
  public startConnection = (options: Options) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(window.location.origin + '/notifications')
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => {

        this.LandRequestAsync(options);
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  public MoveRequestAsync(guid: string, command: string) {
    this.hubConnection.invoke('MoveRequestAsync', guid, command);
  }

  public LandRequestAsync(options: Options) {
    console.log('Invoke LandRequestAsync', options);
    this.hubConnection.invoke('LandRequestAsync', options);
  }
  public registerOnServerEvents() {

    this.hubConnection.on(
      'StateResponseAsync',
      (data: IState) => {
        console.log('Listening StateResponseAsync - received', data);
        this.simpleStateService.update(data);
      });
  }
}
