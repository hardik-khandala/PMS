import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  open: boolean = false;
  openNavbar: boolean = false;
  constructor(public auth: AuthService) { }
  openMenu() {
    this.open = !this.open;
  }

  openNavbarMenu() {
    this.openNavbar = !this.openNavbar
  }
}
