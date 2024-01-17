CREATE DATABASE TaskManager;
GO;

USE TaskManager;
GO;

CREATE TABLE Priorities(
ID INT PRIMARY KEY IDENTITY,
PriorityName NVARCHAR(20),
)

INSERT INTO Priorities(PriorityName) VALUES
	('Baja'),('Media'),('Alta'),('Urgente')

CREATE TABLE Tasks (
    ID INT PRIMARY KEY IDENTITY,
    Description NVARCHAR(MAX),
    CreationDate DATETIME,
    Status BIT,
    Priority INT FOREIGN KEY REFERENCES Priorities(ID)
);
GO;

CREATE PROCEDURE GetPriorityList
AS
BEGIN
    SELECT * FROM Priorities Order By ID ASC
	;
END;
GO;

CREATE PROCEDURE GetTasks
AS
BEGIN
    SELECT * FROM Tasks WHERE Status = 1 Order By Priority DESC
	;
END;
GO;

CREATE PROCEDURE GetTaskById
    @TaskId INT
AS
BEGIN
    SELECT * FROM Tasks WHERE ID = @TaskId AND Status = 1;
END;
GO;

CREATE PROCEDURE AddTask
    @Description NVARCHAR(MAX),
    @Priority INT
AS
BEGIN
    INSERT INTO Tasks (Description, CreationDate, Status, Priority)
    VALUES (@Description, GETDATE(), 1, @Priority);
END;
GO;

ALTER PROCEDURE UpdateTask
    @TaskId INT,
    @Description NVARCHAR(MAX),
    @Priority INT
AS
BEGIN
    UPDATE Tasks
    SET Description = @Description,
        Priority = @Priority
    WHERE ID = @TaskId;
END;

GO;

CREATE PROCEDURE RemoveTask
    @TaskId INT
AS
BEGIN
    UPDATE Tasks
    SET Status = 0
    WHERE ID = @TaskId;
END;

