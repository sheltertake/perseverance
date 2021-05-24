import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { IState } from '../app.component';
import { SimpleStateService } from './simple-state.service';
// import { MessagePackHubProtocol } from '@microsoft/signalr-protocol-msgpack';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor(private simpleStateService: SimpleStateService) {
      
  }
  private hubConnection!: signalR.HubConnection;
  private counter = 0;
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/notifications')
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Invoke LandRequestAsync');
        this.hubConnection.invoke('LandRequestAsync');
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  public MoveRequestAsync(guid: string, command: string) {
    this.hubConnection.invoke('MoveRequestAsync', guid, command);
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
