import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: HubConnection;

  private messageReceived = new Subject<{ user: string; message: string }>();

  messageReceived$ = this.messageReceived.asObservable();

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5251/chatHub') // Adjust the URL based on your API endpoint
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.hubConnection.on('ReceiveMessage', (user, message) => {
      this.messageReceived.next({ user, message });
    });
  }

  sendMessage(user: string, message: string): void {
    this.hubConnection
      .invoke('SendMessage', user, message)
      .catch((err) => console.error(err));
  }
}
