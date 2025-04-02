export interface Criteria {
    criteriaId: number,
    criteriaName: string,
    details: string
}

export interface Department {
    deptId: number,
    deptName: string
}

export interface ManagerList {
    empId: number,
    empName: string
}

export interface Role {
    roleId: number,
    roleName: string
}

export interface Notification {
    notificationId: number,
    message: string,
    createdAt: string
}

export interface DashboardData { 
    totalEmployee: number, 
    totalReview: number, 
    totalGoal: number, 
    totalReport: number 
}