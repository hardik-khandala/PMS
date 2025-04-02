CREATE PROCEDURE GetEmployeeData
    @Search NVARCHAR(100) = NULL,   
    @DeptId INT = NULL,             
    @PageNumber INT = 1,             
    @PageSize INT = 5               
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Skip INT, @Take INT;
    SET @Skip = (@PageNumber - 1) * @PageSize;
    SET @Take = @PageSize;

    DECLARE @Total INT;

    SELECT @Total = COUNT(*) 
    FROM Employees e
    LEFT JOIN Department d ON e.DeptId = d.DeptId
    LEFT JOIN RoleTable r ON e.RoleId = r.RoleId
    WHERE e.IsDeleted = 0
      AND (@DeptId IS NULL OR e.DeptId = @DeptId)
      AND (@Search IS NULL OR e.EmpName LIKE '%' + @Search + '%');

    SELECT 
        e.EmpId,
        e.EmpName,
        e.UserName,
        e.Email,
        e.JoiningDate,
        d.DeptName AS Department,
        r.Rolename AS Role
    FROM Employees e
    LEFT JOIN Department d ON e.DeptId = d.DeptId
    LEFT JOIN RoleTable r ON e.RoleId = r.RoleId
    WHERE e.IsDeleted = 0
      AND (@DeptId IS NULL OR e.DeptId = @DeptId)
      AND (@Search IS NULL OR e.EmpName LIKE '%' + @Search + '%')
    ORDER BY e.EmpId
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

    SELECT @Total AS TotalRecords;
END

EXEC GetEmployeeData


CREATE OR ALTER PROCEDURE GetGoalData
	@Search NVARCHAR(100) = NULL,
	@StatusId INT = NULL,
	@EmpId INT 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		g.goalId,
		g.title,
		g.detail,
		s.status,
		g.createdAt,
		g.dueDate
	FROM Goal g
	JOIN StatusTable s
	ON g.statusId = s.id
	WHERE g.empId = @EmpId 
	AND g.isDeleted = 0
	AND (@StatusId IS NULL OR g.statusId = @StatusId)
	AND (@Search IS NULL OR g.title LIKE '%' + @Search + '%' OR g.detail LIKE '%' + @Search + '%')
	ORDER By g.goalId
END

EXEC GetGoalData @EmpId = 3

CREATE OR ALTER PROCEDURE GetReportData
	@startDate DATE = NULL,
	@endDate DATE = NULL,
	@deptId INT = NULL
AS
BEGIN 
	SET NOCOUNT ON;
	SELECT * FROM ReportView 
	WHERE (@deptId IS NULL OR deptId = @deptId)
	AND (@startDate IS NULL OR startDate >= @startDate)
	AND (@endDate IS NULL OR endDate <= @endDate)
END

EXEC GetReportData @startDate = '2025-03-01', @endDate = '2025-03-17' 


CREATE OR ALTER VIEW ReportView
AS
SELECT
	r.reviewId,
	e.empName as EmployeeName,
	m.empName as ManagerName,
	r.startDate,
	r.endDate,
	c.criteriaName,
	r.selfRating,
	r.strength,
	r.improvement,
	r.managerRating,
	r.managerFeedback,
	e.deptId
FROM PerfomanceReview r
JOIN Employees e ON r.empId = e.empId
JOIN Employees m ON r.managerId = m.empId
JOIN PredefinedCriteria c ON r.criteriaId = c.criteriaId
WHERE r.isDeleted = 0

SELECT * FROM ReportView