import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { BaseUrl } from '../config/environment';
import { TokenService } from './token.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { getHeaders } from '../config/headers';
import { BehaviorSubject, catchError, throwError } from 'rxjs';
import { Notification } from '../models/data.model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection!: HubConnection;
  private notificationCount = new BehaviorSubject<number>(0);
  public notificationCount$ = this.notificationCount.asObservable();

  private readonly hubUrl = `${BaseUrl}/notification`;

  constructor(private http: HttpClient, private tokenService: TokenService, private toastr: ToastrService, private ts: TokenService) { }

  public startConnection() {
    const token = this.tokenService.token.value;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}?access_token=${token}`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected!'))
      .catch((err) => console.error('Error while starting SignalR:', err));

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      this.showNotification(message);
    });

  }

  stopConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection
        .stop()
        .then(() => console.log('SignalR Connection Stopped.'))
        .catch((err) => console.error('Error stopping SignalR:', err));
    }
  }

  private showNotification(message: string) {
    this.toastr.show(message, "New Notification");
  }

  getAllNotifications() {
    return this.http.get<{ res: Notification[], count: number }>(BaseUrl + '/api/notification/GetAllNotification', getHeaders(this.ts.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  markAsRead(id: number) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/notification/MarkAsRead/${id}`, {}, getHeaders(this.ts.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  clearAllNotification() {
    return this.http.put<{ msg: string }>(BaseUrl + '/api/notification/ClearAll', {}, getHeaders(this.ts.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
