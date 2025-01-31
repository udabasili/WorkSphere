import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-nav',
  standalone: false,

  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})


export class NavComponent implements  OnInit, OnDestroy{
  windowListenerSub?: Subscription

  ngOnInit(): void {
    window.addEventListener('resize', this.clearEncodedHtml)
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.clearEncodedHtml)
  }

  openSideNav () {
    const sideNav = document.getElementById('side-nav');
    console.log(sideNav,  window.innerWidth)
    if (sideNav && window.innerWidth < 768) {
      sideNav.style.width = '50vw';
    }
  }

  clearEncodedHtml(){
    const sideNav = document.getElementById('side-nav');
    if (sideNav && window.innerWidth > 768) {
      sideNav.style.width = '';
    }
  }


}
