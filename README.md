# Performance Management System
 
## 1. Case Study Background:
 
Organizations need an efficient way to assess employee performance, set goals, and provide feedback for professional growth. This system automates performance evaluations, reducing manual work and improving transparency.
 
### Stakeholders:
✅ **HR/Admin**: Manages evaluation criteria, generates reports.
✅ **Employee**: Views performance reviews, submits self-evaluations.
✅ **Manager**: Evaluates employees and provides feedback.
 
### Key Features:
✅ **Authentication & Authorization** (JWT-based, role-based access)
✅ **Performance Reviews** (Self-review, Manager review)
✅ **Goal Setting** (Set and track personal & organizational goals)
✅ **Feedback System** (Real-time feedback & comments)
✅ **Performance Reports** (Quarterly, yearly, department-wise)
✅ **Audit Logs & Notifications** (Track changes & updates)
 
## 2. Business Logic
 
### (A) Authentication & Authorization
🔹 Secure JWT-based authentication
🔹 Roles: Admin, Employee, Manager, HR
🔹 Employees can only view their own evaluations
🔹 Managers/Admins can review employees
 
### (B) Employee Self-Review
📌 **Features:**
✅ Submit self-evaluation for the review period
✅ Rate performance based on predefined criteria
✅ Add comments and improvement areas
 
📌 **Review Fields & Control IDs:**
| Field | Control ID |
|-------|------------|
| Review Period | ddlReviewPeriod |
| Self-Rating | txtSelfRating |
| Strengths | txtStrengths |
| Areas for Improvement | txtImprovements |
| Submit Button | btnSubmitReview |
 
📌 **Errors & Control IDs:**
| Error Message | Error Control ID |
|---------------|------------------|
| "Please select a review period" | errSelectReviewPeriod |
| "Rating is required" | errRatingRequired |
| "Comments cannot be empty" | errCommentsRequired |
 
📌 **Audit Logs:**
Each self-review logs:
- Employee Name
- Review Period
- Ratings & Comments
- Submission Timestamp
 
### (C) Manager Review & Feedback
📌 **Features:**
✅ Evaluate employee performance
✅ Provide feedback & development recommendations
✅ Approve or request revision of self-evaluation
 
📌 **Manager Review Fields & Control IDs:**
| Field | Control ID |
|-------|------------|
| Employee Name | lblEmployeeName |
| Review Period | lblReviewPeriod |
| Manager Rating | txtManagerRating |
| Feedback | txtManagerFeedback |
| Approve Button | btnApproveReview |
| Request Revision Button | btnRequestRevision |
 
📌 **Errors & Control IDs:**
| Error Message | Error Control ID |
|---------------|------------------|
| "Please provide a rating" | errManagerRating |
| "Feedback is required" | errManagerFeedback |
 
📌 **Audit Logs:**
Every review logs:
- Employee Name
- Review Period
- Manager Rating & Comments
- Approval Status
- Timestamp
 
### (D) Employee Dashboard
📌 **Sections & Control IDs:**
| Section | Control ID |
|---------|------------|
| Welcome Message | lblWelcome |
| Submit Review | btnSubmitReview |
| View Feedback | grdFeedbackHistory |
 
📌 **Errors & Control IDs:**
| Error Message | Error Control ID |
|---------------|------------------|
| "No performance records found" | errNoPerformanceRecords |

 
## 3. Additional Enhancements
✅ **Performance History Logs** – Track all evaluations and feedback
✅ **Approval Workflow** – Multi-level reviews (Manager → HR → Admin)
✅ **Role-based Access** – Employees review themselves, Managers evaluate
✅ **Real-time Notifications** – Employees get feedback alerts
✅ **Reports & Analytics** – HR/Admin can track performance trends
 
## 4. Client-Side Routing (Angular)
 
```typescript
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: EmployeeDashboardComponent, canActivate: [AuthGuard] },
  { path: 'self-review', component: SelfReviewComponent, canActivate: [AuthGuard] },
  { path: 'manager-review', component: ManagerReviewComponent, canActivate: [AuthGuard, ManagerGuard] },
  { path: 'performance-reports', component: ReportsComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: '**', component: NotFoundComponent }
];
 
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```
 
**Explanation:**
✅ **Login** (/login) – Public route for authentication
✅ **Dashboard** (/dashboard) – Employees view their performance
✅ **Self-Review** (/self-review) – Employees submit self-evaluations
✅ **Manager Review** (/manager-review) – Managers review employees
✅ **Performance Reports** (/performance-reports) – HR/Admin track performance trends
✅ **404 Page** (/**) – Redirects to Not Found
