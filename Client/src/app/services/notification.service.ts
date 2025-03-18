import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { BaseUrl } from '../config/environment';
import { TokenService } from './token.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: HubConnection

  constructor(private http: HttpClient, private tokenService: TokenService) {
    this.startConnection();
   }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(BaseUrl + `/notification?access_token=${this.tokenService.token.value}`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start()
      .catch(err => console.log(err))

    this.hubConnection.on("ReceiveNotification", (message: string) => {
      console.log(message);
      
    })  
  }
}
