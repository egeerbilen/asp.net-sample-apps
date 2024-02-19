import { Component } from '@angular/core';
import { SignalRService } from './service/SignalR.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  user: string = '';
  message: string = '';
  messages: { user: string; message: string }[] = [];

  constructor(private signalRService: SignalRService) {
    this.signalRService.messageReceived$.subscribe(
      (msg: { user: string; message: string }) => {
        this.messages.push(msg);
      }
    );
  }

  sendMessage(): void {
    if (this.user && this.message) {
      this.signalRService.sendMessage(this.user, this.message);
      this.message = '';
    }
  }
}
