﻿/*
Deployment script for timetableCreation

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "timetableCreation"
:setvar DefaultFilePrefix "timetableCreation"
:setvar DefaultDataPath "C:\Users\Jack\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Jack\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping [dbo].[FK_Lecturer]...';


GO
ALTER TABLE [dbo].[Timetable] DROP CONSTRAINT [FK_Lecturer];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Dropping [dbo].[FK_Room]...';


GO
ALTER TABLE [dbo].[Timetable] DROP CONSTRAINT [FK_Room];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Dropping [dbo].[FK_Course]...';


GO
ALTER TABLE [dbo].[Timetable] DROP CONSTRAINT [FK_Course];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Starting rebuilding table [dbo].[Timetable]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Timetable] (
    [id]         INT         IDENTITY (1, 1) NOT NULL,
    [courseId]   INT         NOT NULL,
    [moduleId]   INT         NOT NULL,
    [lecturerId] INT         NULL,
    [roomId]     INT         NOT NULL,
    [time]       VARCHAR (5) NOT NULL,
    [day]        VARCHAR (9) NOT NULL,
    [duration]   INT         NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Timetable])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Timetable] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Timetable] ([id], [courseId], [moduleId], [lecturerId], [roomId], [time], [day], [duration])
        SELECT   [id],
                 [courseId],
                 [moduleId],
                 [lecturerId],
                 [roomId],
                 [time],
                 [day],
                 [duration]
        FROM     [dbo].[Timetable]
        ORDER BY [id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Timetable] OFF;
    END

DROP TABLE [dbo].[Timetable];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Timetable]', N'Timetable';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[Course]...';



GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[Room]...';





GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_Room]...';


GO
ALTER TABLE [dbo].[Timetable] WITH NOCHECK
    ADD CONSTRAINT [FK_Room] FOREIGN KEY ([roomId]) REFERENCES [dbo].[Room] ([roomId]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_Course]...';


GO
ALTER TABLE [dbo].[Timetable] WITH NOCHECK
    ADD CONSTRAINT [FK_Course] FOREIGN KEY ([courseId]) REFERENCES [dbo].[Course] ([courseId]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'The transacted portion of the database update succeeded.'
COMMIT TRANSACTION
END
ELSE PRINT N'The transacted portion of the database update failed.'
GO
DROP TABLE #tmpErrors
GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Timetable] WITH CHECK CHECK CONSTRAINT [FK_Room];

ALTER TABLE [dbo].[Timetable] WITH CHECK CHECK CONSTRAINT [FK_Course];


GO
PRINT N'Update complete.';


GO