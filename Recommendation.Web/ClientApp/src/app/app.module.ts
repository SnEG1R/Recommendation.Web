import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component';
import {RegistrationComponent} from "./registration/registration.component";
import {HeaderComponent} from "./header/header.component";
import {LoginComponent} from "./login/login.component";
import {ThemeToggleComponent} from "./theme-toggle/theme-toggle.component";
import {LoginCallbackComponent} from "./login-callback/login-callback.component";
import {ExternalLoginComponent} from "./external-login/external-login.component";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegistrationComponent,
    LoginComponent,
    HeaderComponent,
    ThemeToggleComponent,
    LoginCallbackComponent,
    ExternalLoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'login-callback', component: LoginCallbackComponent},
      {path: 'registration', component: RegistrationComponent},
      {path: 'login', component: LoginComponent}
    ]),
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
