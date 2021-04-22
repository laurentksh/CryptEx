import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './Components/home/home.component';
import { ContactComponent } from './Components/contact/contact.component';



@NgModule({
  declarations: [HomeComponent, ContactComponent],
  imports: [
    CommonModule
  ]
})
export class MainModule { }
