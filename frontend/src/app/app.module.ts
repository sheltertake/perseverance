import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent, TrisColorPipe, TrisPipe } from './app.component';

@NgModule({
  declarations: [
    TrisPipe,
    TrisColorPipe,
    AppComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
