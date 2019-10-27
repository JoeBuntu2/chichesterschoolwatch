import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, UrlSerializer } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LowerCaseUrlSerializer } from './lower-case-url-serializer';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    NgbModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      {
        path: "employees",
        loadChildren: "./employees/employees.module#EmployeesModule"
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
