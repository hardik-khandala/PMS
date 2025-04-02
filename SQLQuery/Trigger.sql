CREATE TRIGGER trg_AuditLog_Employees
ON Employees
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        i.empId, 
        'INSERT', 
        'Employees', 
        NULL, 
        CONCAT(
            CASE WHEN i.empName IS NOT NULL THEN 'empName: ' + i.empName ELSE '' END,
            CASE WHEN i.userName IS NOT NULL THEN ', userName: ' + i.userName ELSE '' END,
            CASE WHEN i.email IS NOT NULL THEN ', email: ' + i.email ELSE '' END,
            CASE WHEN i.deptId IS NOT NULL THEN ', deptId: ' + CAST(i.deptId AS VARCHAR) ELSE '' END,
            CASE WHEN i.roleId IS NOT NULL THEN ', roleId: ' + CAST(i.roleId AS VARCHAR) ELSE '' END,
            CASE WHEN i.managerId IS NOT NULL THEN ', managerId: ' + CAST(i.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN i.joiningDate IS NOT NULL THEN ', joiningDate: ' + CAST(i.joiningDate AS VARCHAR) ELSE '' END,
            CASE WHEN i.isDeleted IS NOT NULL THEN ', isDeleted: ' + CAST(i.isDeleted AS VARCHAR) ELSE '' END
        ),
        i.createdBy,  
        GETDATE()
    FROM inserted i
    WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.empId = i.empId);

    -- Update Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        i.empId, 
        CASE 
            WHEN i.isDeleted = 1 AND d.isDeleted = 0 THEN 'DELETE' 
            WHEN i.isDeleted = 0 AND d.isDeleted = 1 THEN 'RESTORE'  -- When isDeleted changes from TRUE to FALSE
            ELSE 'UPDATE' 
        END,
        'Employees', 
        CONCAT(
            CASE WHEN i.empName != d.empName THEN 'empName: ' + d.empName ELSE '' END,
            CASE WHEN i.userName != d.userName THEN ', userName: ' + d.userName ELSE '' END,
            CASE WHEN i.email != d.email THEN ', email: ' + d.email ELSE '' END,
            CASE WHEN i.deptId != d.deptId THEN ', deptId: ' + CAST(d.deptId AS VARCHAR) ELSE '' END,
            CASE WHEN i.roleId != d.roleId THEN ', roleId: ' + CAST(d.roleId AS VARCHAR) ELSE '' END,
            CASE WHEN i.managerId != d.managerId THEN ', managerId: ' + CAST(d.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN i.joiningDate != d.joiningDate THEN ', joiningDate: ' + CAST(d.joiningDate AS VARCHAR) ELSE '' END,
            CASE WHEN i.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(d.isDeleted AS VARCHAR) ELSE '' END
        ),
        CONCAT(
            CASE WHEN i.empName != d.empName THEN 'empName: ' + i.empName ELSE '' END,
            CASE WHEN i.userName != d.userName THEN ', userName: ' + i.userName ELSE '' END,
            CASE WHEN i.email != d.email THEN ', email: ' + i.email ELSE '' END,
            CASE WHEN i.deptId != d.deptId THEN ', deptId: ' + CAST(i.deptId AS VARCHAR) ELSE '' END,
            CASE WHEN i.roleId != d.roleId THEN ', roleId: ' + CAST(i.roleId AS VARCHAR) ELSE '' END,
            CASE WHEN i.managerId != d.managerId THEN ', managerId: ' + CAST(i.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN i.joiningDate != d.joiningDate THEN ', joiningDate: ' + CAST(i.joiningDate AS VARCHAR) ELSE '' END,
            CASE WHEN i.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(i.isDeleted AS VARCHAR) ELSE '' END
        ),
        i.createdBy,  
        GETDATE()
    FROM inserted i
    JOIN deleted d ON i.empId = d.empId
    WHERE EXISTS (SELECT 1 FROM deleted WHERE deleted.empId = i.empId AND (
        i.empName != d.empName OR i.userName != d.userName OR i.email != d.email OR 
        i.deptId != d.deptId OR i.roleId != d.roleId OR i.managerId != d.managerId OR
        i.joiningDate != d.joiningDate OR i.isDeleted != d.isDeleted));

    -- Delete Log (Soft delete as isDeleted = 1)
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        d.empId, 
        'DELETE', 
        'Employees', 
        CONCAT(
            CASE WHEN d.empName IS NOT NULL THEN 'empName: ' + d.empName ELSE '' END,
            CASE WHEN d.userName IS NOT NULL THEN ', userName: ' + d.userName ELSE '' END,
            CASE WHEN d.email IS NOT NULL THEN ', email: ' + d.email ELSE '' END,
            CASE WHEN d.deptId IS NOT NULL THEN ', deptId: ' + CAST(d.deptId AS VARCHAR) ELSE '' END,
            CASE WHEN d.roleId IS NOT NULL THEN ', roleId: ' + CAST(d.roleId AS VARCHAR) ELSE '' END,
            CASE WHEN d.managerId IS NOT NULL THEN ', managerId: ' + CAST(d.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN d.joiningDate IS NOT NULL THEN ', joiningDate: ' + CAST(d.joiningDate AS VARCHAR) ELSE '' END,
            CASE WHEN d.isDeleted IS NOT NULL THEN ', isDeleted: ' + CAST(d.isDeleted AS VARCHAR) ELSE '' END
        ),
        NULL,  -- For DELETE, newValue is NULL
        d.createdBy,  
        GETDATE()
    FROM deleted d
    WHERE d.isDeleted = 1 -- Only log DELETE for soft deletes
    AND NOT EXISTS (SELECT 1 FROM inserted i WHERE i.empId = d.empId);
END;


CREATE TRIGGER trg_AuditLog_Goal
ON Goal
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        g.empId, 
        'INSERT', 
        'Goal', 
        NULL, 
        CONCAT(
            'empId: ', g.empId, 
            ', title: ', g.title, 
            ', detail: ', g.detail, 
            ', statusId: ', g.statusId, 
            ', createdBy: ', g.createdBy, 
            ', createdAt: ', g.createdAt, 
            ', isDeleted: ', g.isDeleted, 
            ', modifyBy: ', g.modifyBy, 
            ', modifyAt: ', g.modifyAt, 
            ', dueDate: ', g.dueDate
        ),
        g.createdBy,  
        GETDATE()
    FROM inserted g
    WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.goalId = g.goalId);

    -- Update Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        g.empId, 
        CASE 
            WHEN g.isDeleted = 1 AND d.isDeleted = 0 THEN 'DELETE' 
            WHEN g.isDeleted = 0 AND d.isDeleted = 1 THEN 'RESTORE'  -- If soft delete is removed
            ELSE 'UPDATE' 
        END,
        'Goal', 
        CONCAT(
            CASE WHEN g.empId != d.empId THEN 'empId: ' + CAST(d.empId AS VARCHAR) ELSE '' END,
            CASE WHEN g.title != d.title THEN ', title: ' + d.title ELSE '' END,
            CASE WHEN g.detail != d.detail THEN ', detail: ' + d.detail ELSE '' END,
            CASE WHEN g.statusId != d.statusId THEN ', statusId: ' + CAST(d.statusId AS VARCHAR) ELSE '' END,
            CASE WHEN g.createdBy != d.createdBy THEN ', createdBy: ' + CAST(d.createdBy AS VARCHAR) ELSE '' END,
            CASE WHEN g.createdAt != d.createdAt THEN ', createdAt: ' + CAST(d.createdAt AS VARCHAR) ELSE '' END,
            CASE WHEN g.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(d.isDeleted AS VARCHAR) ELSE '' END,
            CASE WHEN g.modifyBy != d.modifyBy THEN ', modifyBy: ' + CAST(d.modifyBy AS VARCHAR) ELSE '' END,
            CASE WHEN g.modifyAt != d.modifyAt THEN ', modifyAt: ' + CAST(d.modifyAt AS VARCHAR) ELSE '' END,
            CASE WHEN g.dueDate != d.dueDate THEN ', dueDate: ' + CAST(d.dueDate AS VARCHAR) ELSE '' END
        ),
        CONCAT(
            CASE WHEN g.empId != d.empId THEN 'empId: ' + CAST(g.empId AS VARCHAR) ELSE '' END,
            CASE WHEN g.title != d.title THEN ', title: ' + g.title ELSE '' END,
            CASE WHEN g.detail != d.detail THEN ', detail: ' + g.detail ELSE '' END,
            CASE WHEN g.statusId != d.statusId THEN ', statusId: ' + CAST(g.statusId AS VARCHAR) ELSE '' END,
            CASE WHEN g.createdBy != d.createdBy THEN ', createdBy: ' + CAST(g.createdBy AS VARCHAR) ELSE '' END,
            CASE WHEN g.createdAt != d.createdAt THEN ', createdAt: ' + CAST(g.createdAt AS VARCHAR) ELSE '' END,
            CASE WHEN g.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(g.isDeleted AS VARCHAR) ELSE '' END,
            CASE WHEN g.modifyBy != d.modifyBy THEN ', modifyBy: ' + CAST(g.modifyBy AS VARCHAR) ELSE '' END,
            CASE WHEN g.modifyAt != d.modifyAt THEN ', modifyAt: ' + CAST(g.modifyAt AS VARCHAR) ELSE '' END,
            CASE WHEN g.dueDate != d.dueDate THEN ', dueDate: ' + CAST(g.dueDate AS VARCHAR) ELSE '' END
        ),
        g.modifyBy,  
        GETDATE()
    FROM inserted g
    JOIN deleted d ON g.goalId = d.goalId
    WHERE EXISTS (SELECT 1 FROM deleted WHERE deleted.goalId = g.goalId AND (
        g.empId != d.empId OR g.title != d.title OR g.detail != d.detail OR 
        g.statusId != d.statusId OR g.createdBy != d.createdBy OR g.createdAt != d.createdAt OR 
        g.isDeleted != d.isDeleted OR g.modifyBy != d.modifyBy OR g.modifyAt != d.modifyAt OR 
        g.dueDate != d.dueDate));

    -- Delete Log (Soft delete as isDeleted = 1)
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        d.empId, 
        'DELETE', 
        'Goal', 
        CONCAT(
            'empId: ', d.empId, 
            ', title: ', d.title, 
            ', detail: ', d.detail, 
            ', statusId: ', d.statusId, 
            ', createdBy: ', d.createdBy, 
            ', createdAt: ', d.createdAt, 
            ', isDeleted: ', d.isDeleted, 
            ', modifyBy: ', d.modifyBy, 
            ', modifyAt: ', d.modifyAt, 
            ', dueDate: ', d.dueDate
        ),
        NULL,  -- For DELETE, newValue is NULL
        d.modifyBy,  
        GETDATE()
    FROM deleted d
    WHERE d.isDeleted = 1 -- Only log DELETE for soft deletes
    AND NOT EXISTS (SELECT 1 FROM inserted g WHERE g.goalId = d.goalId);
END;





CREATE TRIGGER trg_AuditLog_PerfomanceReview
ON PerfomanceReview
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        pr.empId, 
        'INSERT', 
        'PerfomanceReview', 
        NULL, 
        CONCAT(
            'empId: ', pr.empId, 
            ', managerId: ', pr.managerId, 
            ', startDate: ', pr.startDate, 
            ', endDate: ', pr.endDate, 
            ', criteriaId: ', pr.criteriaId, 
            ', selfRating: ', pr.selfRating, 
            ', strength: ', pr.strength, 
            ', improvement: ', pr.improvement, 
            ', statusId: ', pr.statusId, 
            ', managerRating: ', pr.managerRating, 
            ', managerFeedback: ', pr.managerFeedback, 
            ', createdAt: ', pr.createdAt, 
            ', createdBy: ', pr.createdBy, 
            ', isDeleted: ', pr.isDeleted, 
            ', modifyBy: ', pr.modifyBy, 
            ', modifyAt: ', pr.modifyAt
        ),
        pr.createdBy,  
        GETDATE()
    FROM inserted pr
    WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.reviewId = pr.reviewId);

    -- Update Log
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        pr.empId, 
        CASE 
            WHEN pr.isDeleted = 1 AND d.isDeleted = 0 THEN 'DELETE' 
            WHEN pr.isDeleted = 0 AND d.isDeleted = 1 THEN 'RESTORE'  -- If soft delete is removed
            ELSE 'UPDATE' 
        END,
        'PerfomanceReview', 
        CONCAT(
            CASE WHEN pr.empId != d.empId THEN 'empId: ' + CAST(d.empId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerId != d.managerId THEN ', managerId: ' + CAST(d.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.startDate != d.startDate THEN ', startDate: ' + CAST(d.startDate AS VARCHAR) ELSE '' END,
            CASE WHEN pr.endDate != d.endDate THEN ', endDate: ' + CAST(d.endDate AS VARCHAR) ELSE '' END,
            CASE WHEN pr.criteriaId != d.criteriaId THEN ', criteriaId: ' + CAST(d.criteriaId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.selfRating != d.selfRating THEN ', selfRating: ' + CAST(d.selfRating AS VARCHAR) ELSE '' END,
            CASE WHEN pr.strength != d.strength THEN ', strength: ' + d.strength ELSE '' END,
            CASE WHEN pr.improvement != d.improvement THEN ', improvement: ' + d.improvement ELSE '' END,
            CASE WHEN pr.statusId != d.statusId THEN ', statusId: ' + CAST(d.statusId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerRating != d.managerRating THEN ', managerRating: ' + CAST(d.managerRating AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerFeedback != d.managerFeedback THEN ', managerFeedback: ' + d.managerFeedback ELSE '' END,
            CASE WHEN pr.createdAt != d.createdAt THEN ', createdAt: ' + CAST(d.createdAt AS VARCHAR) ELSE '' END,
            CASE WHEN pr.createdBy != d.createdBy THEN ', createdBy: ' + CAST(d.createdBy AS VARCHAR) ELSE '' END,
            CASE WHEN pr.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(d.isDeleted AS VARCHAR) ELSE '' END,
            CASE WHEN pr.modifyBy != d.modifyBy THEN ', modifyBy: ' + CAST(d.modifyBy AS VARCHAR) ELSE '' END,
            CASE WHEN pr.modifyAt != d.modifyAt THEN ', modifyAt: ' + CAST(d.modifyAt AS VARCHAR) ELSE '' END
        ),
        CONCAT(
            CASE WHEN pr.empId != d.empId THEN 'empId: ' + CAST(pr.empId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerId != d.managerId THEN ', managerId: ' + CAST(pr.managerId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.startDate != d.startDate THEN ', startDate: ' + CAST(pr.startDate AS VARCHAR) ELSE '' END,
            CASE WHEN pr.endDate != d.endDate THEN ', endDate: ' + CAST(pr.endDate AS VARCHAR) ELSE '' END,
            CASE WHEN pr.criteriaId != d.criteriaId THEN ', criteriaId: ' + CAST(pr.criteriaId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.selfRating != d.selfRating THEN ', selfRating: ' + CAST(pr.selfRating AS VARCHAR) ELSE '' END,
            CASE WHEN pr.strength != d.strength THEN ', strength: ' + pr.strength ELSE '' END,
            CASE WHEN pr.improvement != d.improvement THEN ', improvement: ' + pr.improvement ELSE '' END,
            CASE WHEN pr.statusId != d.statusId THEN ', statusId: ' + CAST(pr.statusId AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerRating != d.managerRating THEN ', managerRating: ' + CAST(pr.managerRating AS VARCHAR) ELSE '' END,
            CASE WHEN pr.managerFeedback != d.managerFeedback THEN ', managerFeedback: ' + pr.managerFeedback ELSE '' END,
            CASE WHEN pr.createdAt != d.createdAt THEN ', createdAt: ' + CAST(pr.createdAt AS VARCHAR) ELSE '' END,
            CASE WHEN pr.createdBy != d.createdBy THEN ', createdBy: ' + CAST(pr.createdBy AS VARCHAR) ELSE '' END,
            CASE WHEN pr.isDeleted != d.isDeleted THEN ', isDeleted: ' + CAST(pr.isDeleted AS VARCHAR) ELSE '' END,
            CASE WHEN pr.modifyBy != d.modifyBy THEN ', modifyBy: ' + CAST(pr.modifyBy AS VARCHAR) ELSE '' END,
            CASE WHEN pr.modifyAt != d.modifyAt THEN ', modifyAt: ' + CAST(pr.modifyAt AS VARCHAR) ELSE '' END
        ),
        pr.modifyBy,  
        GETDATE()
    FROM inserted pr
    JOIN deleted d ON pr.reviewId = d.reviewId
    WHERE EXISTS (SELECT 1 FROM deleted WHERE deleted.reviewId = pr.reviewId AND (
        pr.empId != d.empId OR pr.managerId != d.managerId OR pr.startDate != d.startDate OR 
        pr.endDate != d.endDate OR pr.criteriaId != d.criteriaId OR pr.selfRating != d.selfRating OR 
        pr.strength != d.strength OR pr.improvement != d.improvement OR pr.statusId != d.statusId OR 
        pr.managerRating != d.managerRating OR pr.managerFeedback != d.managerFeedback OR 
        pr.createdAt != d.createdAt OR pr.createdBy != d.createdBy OR pr.isDeleted != d.isDeleted OR 
        pr.modifyBy != d.modifyBy OR pr.modifyAt != d.modifyAt));

    -- Delete Log (Soft delete as isDeleted = 1)
    INSERT INTO AuditLogs (empId, action, tableName, oldValue, newValue, changedBy, changedAt)
    SELECT 
        d.empId, 
        'DELETE', 
        'PerfomanceReview', 
        CONCAT(
            'empId: ', d.empId, 
            ', managerId: ', d.managerId, 
            ', startDate: ', d.startDate, 
            ', endDate: ', d.endDate, 
            ', criteriaId: ', d.criteriaId, 
            ', selfRating: ', d.selfRating, 
            ', strength: ', d.strength, 
            ', improvement: ', d.improvement, 
            ', statusId: ', d.statusId, 
            ', managerRating: ', d.managerRating, 
            ', managerFeedback: ', d.managerFeedback, 
            ', createdAt: ', d.createdAt, 
            ', createdBy: ', d.createdBy, 
            ', isDeleted: ', d.isDeleted, 
            ', modifyBy: ', d.modifyBy, 
            ', modifyAt: ', d.modifyAt
        ),
        NULL,  -- For DELETE, newValue is NULL
        d.modifyBy,  
        GETDATE()
    FROM deleted d
    WHERE d.isDeleted = 1 -- Only log DELETE for soft deletes
    AND NOT EXISTS (SELECT 1 FROM inserted pr WHERE pr.reviewId = d.reviewId);
END;
