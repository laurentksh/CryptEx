import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit {
  isOpen = false;

  constructor() { 

  }

  ngOnInit(): void {
   
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    var el = event.target as HTMLElement;
    console.log(el?.className)
    console.log(el.parentElement?.className)
    if (el == null)
      return;
    
    if(this.isOpen && !el.parentElement?.className.includes("user-dropdown")){
      if (el.parentElement?.className.includes("dp-btn")) {
        return;
      }

      this.isOpen = false;
    }
  }
}
