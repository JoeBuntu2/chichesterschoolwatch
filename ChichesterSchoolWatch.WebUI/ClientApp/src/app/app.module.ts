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

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent, 
    AboutComponent
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
        path: "budgets",
        loadChildren: "./budgets/budgets.module#BudgetsModule"
      },
      {
        path: "tutorials",
        loadChildren: "./tutorial/tutorial.module#TutorialModule"
      }
    ])
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
