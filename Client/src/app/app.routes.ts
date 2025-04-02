import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { EmployeeDashboardComponent } from './components/employee-dashboard/employee-dashboard.component';
import { SelfReviewComponent } from './components/self-review/self-review.component';
import { ManagerReviewComponent } from './components/manager-review/manager-review.component';
import { ReportsComponent } from './components/reports/reports.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { authGuard } from './guards/auth.guard';
import { YourreviewComponent } from './components/yourreview/yourreview.component';
import { RegisterComponent } from './components/register/register.component';
import { GoalComponent } from './components/goal/goal.component';
import { AddGoalComponent } from './components/add-goal/add-goal.component';
import { AddCriteriaComponent } from './components/add-criteria/add-criteria.component';

export const routes: Routes = [
  {
    path: "",
    redirectTo: "/login",
    pathMatch: "full"
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "register",
    component: RegisterComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin'] } 
  },
  {
    path: "employee/edit/:id",
    component: RegisterComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin'] } 
  },
  {
    path: "dashboard",
    component: EmployeeDashboardComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] } 
  },
  {
    path: "self-review",
    component: SelfReviewComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "self-review/edit/:id",
    component: SelfReviewComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "add-criteria",
    component: AddCriteriaComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: "goals",
    component: GoalComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "goals/add",
    component: AddGoalComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "goals/edit/:id",
    component: AddGoalComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "manager-review",
    component: ManagerReviewComponent,
    canActivate: [authGuard],
    data: { roles: ['Manager'] } 
  },
  {
    path: "performance-report",
    component: ReportsComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'HR'] } 
  },
  {
    path: "reviews",
    component: YourreviewComponent,
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Employee', 'Manager', 'HR'] }
  },
  {
    path: "**",
    component: NotFoundComponent
  }
];
