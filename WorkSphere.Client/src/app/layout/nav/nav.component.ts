import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../features/auth/services/auth.service';
import {User} from '../../features/auth/models/user';

@Component({
  selector: 'app-nav',
  standalone: false,

  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})


export class NavComponent implements OnInit, OnDestroy {
  @Input() userIsAuthenticated: boolean = false;
  @Input() user: User | null = null;

  constructor(
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    window.addEventListener('resize', this.clearEncodedHtml)
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.clearEncodedHtml)
  }

  openSideNav() {
    const sideNav = document.getElementById('side-nav');
    console.log(sideNav, window.innerWidth)
    if (sideNav && window.innerWidth < 768) {
      sideNav.style.width = '50vw';
    }
  }

  clearEncodedHtml() {
    const sideNav = document.getElementById('side-nav');
    if (sideNav && window.innerWidth > 768) {
      sideNav.style.width = '';
    }
  }

  logout() {
    this.authService.logout();
  }


}
