/* HeuristicLab
 * Copyright (C) 2002-2015 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */

/* this script is supposed to be executed after the plain DB is generated by the linq-to-sql schema */
USE [HeuristicLab.Hive-3.3]

ALTER TABLE [dbo].[AssignedResources]  DROP  CONSTRAINT [Task_AssignedResource]
ALTER TABLE [dbo].[AssignedResources]  WITH CHECK ADD  CONSTRAINT [Task_AssignedResource] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([TaskId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssignedResources]  DROP  CONSTRAINT [Resource_AssignedResource]
ALTER TABLE [dbo].[AssignedResources]  WITH CHECK ADD  CONSTRAINT [Resource_AssignedResource] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resource] ([ResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE dbo.Task ALTER COLUMN TaskId ADD ROWGUIDCOL;
ALTER TABLE dbo.Task WITH NOCHECK ADD CONSTRAINT [DF_Task_TaskId] DEFAULT (NEWSEQUENTIALID()) FOR TaskId;
GO

ALTER TABLE [dbo].[StateLog]  DROP  CONSTRAINT [Task_StateLog]
ALTER TABLE [dbo].[StateLog]  WITH CHECK ADD CONSTRAINT [Task_StateLog] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([TaskId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StateLog]  DROP  CONSTRAINT [Resource_StateLog]
ALTER TABLE [dbo].[StateLog]  WITH CHECK ADD CONSTRAINT [Resource_StateLog] FOREIGN KEY([SlaveId])
REFERENCES [dbo].[Resource] ([ResourceId])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE dbo.Plugin ALTER COLUMN PluginId ADD ROWGUIDCOL;
ALTER TABLE dbo.Plugin WITH NOCHECK ADD CONSTRAINT [DF_Plugin_PluginId] DEFAULT (NEWSEQUENTIALID()) FOR PluginId;

ALTER TABLE dbo.PluginData WITH NOCHECK ADD CONSTRAINT [DF_PluginData_PluginDataId] DEFAULT (NEWSEQUENTIALID()) FOR PluginDataId;

ALTER TABLE [dbo].[PluginData]  DROP  CONSTRAINT [Plugin_PluginData]
ALTER TABLE [dbo].[PluginData]  WITH CHECK ADD  CONSTRAINT [Plugin_PluginData] FOREIGN KEY([PluginId])
REFERENCES [dbo].[Plugin] ([PluginId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE dbo.RequiredPlugins ALTER COLUMN RequiredPluginId ADD ROWGUIDCOL;
ALTER TABLE dbo.RequiredPlugins WITH NOCHECK ADD CONSTRAINT [DF_RequiredPlugins_RequiredPluginId] DEFAULT (NEWSEQUENTIALID()) FOR RequiredPluginId;

ALTER TABLE [dbo].[RequiredPlugins]  DROP  CONSTRAINT [Task_RequiredPlugin]
ALTER TABLE [dbo].[RequiredPlugins]  WITH CHECK ADD  CONSTRAINT [Task_RequiredPlugin] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([TaskId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequiredPlugins]  DROP  CONSTRAINT [Plugin_RequiredPlugin]
ALTER TABLE [dbo].[RequiredPlugins]  WITH CHECK ADD  CONSTRAINT [Plugin_RequiredPlugin] FOREIGN KEY([PluginId])
REFERENCES [dbo].[Plugin] ([PluginId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE dbo.Resource ALTER COLUMN ResourceId ADD ROWGUIDCOL;
ALTER TABLE dbo.Resource WITH NOCHECK ADD CONSTRAINT [DF_Resource_ResourceId] DEFAULT (NEWSEQUENTIALID()) FOR ResourceId;

ALTER TABLE dbo.Downtime ALTER COLUMN DowntimeId ADD ROWGUIDCOL;
ALTER TABLE dbo.Downtime WITH NOCHECK ADD CONSTRAINT [DF_Downtime_DowntimeId] DEFAULT (NEWSEQUENTIALID()) FOR DowntimeId;

ALTER TABLE dbo.Job ALTER COLUMN JobId ADD ROWGUIDCOL;
ALTER TABLE dbo.Job WITH NOCHECK ADD CONSTRAINT [DF_Job_JobId] DEFAULT (NEWSEQUENTIALID()) FOR JobId;

ALTER TABLE dbo.StateLog ALTER COLUMN StateLogId ADD ROWGUIDCOL;
ALTER TABLE dbo.StateLog WITH NOCHECK ADD CONSTRAINT [DF_StateLog_StateLogId] DEFAULT (NEWSEQUENTIALID()) FOR StateLogId;

ALTER TABLE [dbo].[JobPermission]  DROP  CONSTRAINT [Job_JobPermission]
ALTER TABLE [dbo].[JobPermission]  WITH CHECK ADD CONSTRAINT [Job_JobPermission] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ResourcePermission]  DROP  CONSTRAINT [Resource_ResourcePermission]
ALTER TABLE [dbo].[ResourcePermission]  WITH CHECK ADD CONSTRAINT [Resource_ResourcePermission] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resource] ([ResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

/* create indices */
CREATE INDEX Index_RequiredPlugins_TaskId ON RequiredPlugins(TaskId);
GO

-- speed up joins between Job and Task
CREATE NONCLUSTERED INDEX [TaskJobIdIndex]
ON [dbo].[Task] ([JobId])
INCLUDE ([TaskId],[TaskState],[ExecutionTimeMs],[LastHeartbeat],[ParentTaskId],[Priority],[CoresNeeded],[MemoryNeeded],[IsParentTask],[FinishWhenChildJobsFinished],[Command])
GO

-- this is an index to speed up the GetWaitingTasks() method 
CREATE NONCLUSTERED INDEX [TaskGetWaitingTasksIndex]
ON [dbo].[Task] ([TaskState],[IsParentTask],[FinishWhenChildJobsFinished],[CoresNeeded],[MemoryNeeded])
INCLUDE ([TaskId],[ExecutionTimeMs],[LastHeartbeat],[ParentTaskId],[Priority],[Command],[JobId])
GO


/****** Object:  Trigger [dbo].[tr_JobDeleteCascade]    Script Date: 04/19/2011 16:31:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		cneumuel
-- Create date: 19.04.2011
-- Description:	(1) Writes the execution times of deleted jobs into DeletedJobStats to ensure correct statistics
--				(2) Deletes all associated jobs. This cannot be done with cascading delete, 
--              because the job table defines a INSTEAD OF DELETE trigger itself, which
--              is not compatible with cascading deletes.
-- =============================================
CREATE TRIGGER [dbo].[tr_JobDeleteCascade] ON [dbo].[Job] INSTEAD OF DELETE AS 
BEGIN
    DELETE Task FROM deleted, Task WHERE deleted.JobId = Task.JobId
    DELETE Job FROM deleted, Job WHERE deleted.JobId = Job.JobId
END
GO

-- =============================================
-- Author:		cneumuel
-- Create date: 11.11.2010
-- Description:	Recursively deletes all child-jobs of a job when it is deleted. (Source: http://devio.wordpress.com/2008/05/23/recursive-delete-in-sql-server/)
-- =============================================DeletedJobStatistics
CREATE TRIGGER [dbo].[tr_TaskDeleteCascade] ON [dbo].[Task] INSTEAD OF DELETE AS 
BEGIN
    -- recursively delete jobs
    CREATE TABLE #Table( 
        TaskId uniqueidentifier 
    )
    INSERT INTO #Table (TaskId)
    SELECT TaskId FROM deleted
    
    DECLARE @c INT
    SET @c = 0
    
    WHILE @c <> (SELECT COUNT(TaskId) FROM #Table) BEGIN
        SELECT @c = COUNT(TaskId) FROM #Table
        
        INSERT INTO #Table (TaskId)
            SELECT Task.TaskId
            FROM Task
            LEFT OUTER JOIN #Table ON Task.TaskId = #Table.TaskId
            WHERE Task.ParentTaskId IN (SELECT TaskId FROM #Table)
                AND #Table.TaskId IS NULL
    END
    
    DELETE TaskData FROM TaskData INNER JOIN #Table ON TaskData.TaskId = #Table.TaskId
    DELETE Task FROM Task INNER JOIN #Table ON Task.TaskId = #Table.TaskId
END
GO

