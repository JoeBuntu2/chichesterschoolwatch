import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, UrlSerializer } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component'; 
import { LowerCaseUrlSerializer } from './lower-case-url-serializer';
import { AboutComponent } from './about/about.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { LetterChichesterPSERSNetContributionsComponent } from './letters/letter-chichester-psers-net-contributions/letter-chichester-psers-net-contributions.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent, 
    AboutComponent, LetterChichesterPSERSNetContributionsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    NgbModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'tutorials/tax-payers-guide-to-high-chi-taxes', pathMatch: 'full' },
      {
        path: "employees",
        loadChildren: "./employees/employees.module#EmployeesModule"
      },
      {
        path: "about",
        component: AboutComponent
      },
      {
        path: "letters/board/psers-net-contributions-accuracy",
        component: LetterChichesterPSERSNetContributionsComponent
      },
      {
        path: "budgets",
        loadChildren: "./budgets/budgets.module#BudgetsModule"
      },
      {
        path: "tutorials",
        loadChildren: "./tutorial/tutorial.module#TutorialModule"
      }
    ]),
    NoopAnimationsModule
  ],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer
    }
    ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
