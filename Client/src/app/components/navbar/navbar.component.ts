import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { TimeAgoPipe } from "../../pipes/time-ago.pipe";
import { TokenService } from '../../services/token.service';
import { Notification } from '../../models/data.model';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, CommonModule, TimeAgoPipe],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  open: boolean = false;
  openNotifications: boolean = false;
  openNavbar: boolean = false;
  notificationList!: Notification[];
  username: string | null = "username";
  count: number = 0;

  constructor(public auth: AuthService, private ns: NotificationService, private toastr: ToastrService, public ts: TokenService) {
    this.username = localStorage.getItem('username');
    this.getNotification();
  }
  openMenu() {
    this.open = !this.open;
  }

  openNavbarMenu() {
    this.openNavbar = !this.openNavbar
  }

  openNotificationMenu() {
    this.openNotifications = !this.openNotifications
    this.getNotification();
  }

  getNotification() {
    this.ns.getAllNotifications().subscribe({
      next: (value) => {
        this.notificationList = value.res;
        this.count = value.count;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }

  markRead(id: number) {
    this.ns.markAsRead(id).subscribe({
      next: (value) => {
        console.log(value);
        this.getNotification();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }
  clearAllNotifications() {
    this.ns.clearAllNotification().subscribe({
      next: (value) => {
        console.log(value);
        this.getNotification();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }
}
