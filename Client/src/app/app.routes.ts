import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { EmployeeDashboardComponent } from './components/employee-dashboard/employee-dashboard.component';
import { SelfReviewComponent } from './components/self-review/self-review.component';
import { ManagerReviewComponent } from './components/manager-review/manager-review.component';
import { ReportsComponent } from './components/reports/reports.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { authGuard } from './guards/auth.guard';
import { managerGuard } from './guards/manager.guard';
import { adminGuard } from './guards/admin.guard';
import { YourreviewComponent } from './components/yourreview/yourreview.component';
import { RegisterComponent } from './components/register/register.component';
import { GoalComponent } from './components/goal/goal.component';

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
        canActivate: [adminGuard]
    },
    {
        path: "employee/edit/:id",
        component: RegisterComponent,
        canActivate: [adminGuard]
    },
    {
        path: "dashboard",
        component: EmployeeDashboardComponent,
        canActivate: [authGuard]
    },
    {
        path: "self-review",
        component: SelfReviewComponent,
        canActivate: [authGuard]
    },
    {
        path: "self-review/edit/:id",
        component: SelfReviewComponent,
        canActivate: [authGuard]
    },
    {
        path: "goals",
        component: GoalComponent,
        canActivate: [authGuard]
    },
    {
        path: "manager-review",
        component: ManagerReviewComponent,
        canActivate: [authGuard, managerGuard]
    },
    {
        path: "performance-report",
        component: ReportsComponent,
        canActivate: [authGuard, adminGuard]
    },
    {
        path: "reviews",
        component: YourreviewComponent
    },
    {
        path: "**",
        component: NotFoundComponent
    },
];
