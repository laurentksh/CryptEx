import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit {
  isOpen = false;
  showMobileMenu = false;

  constructor() { 

  }

  ngOnInit(): void {
   
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    var el = event.target as HTMLElement;
    
    if (el == null || el.nodeName == "path")
      return;
    
    if (el.parentElement?.className?.includes("dp-btn")) {
      this.showMobileMenu = false;
      return;
    }

    if(this.isOpen && !el.parentElement?.className.includes("user-dropdown")){
      this.showMobileMenu = false;
      this.isOpen = false;
    } else {
      this.isOpen = false;
    }
  }
}
