DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'KILL ' + CAST(session_id AS NVARCHAR(10)) + N'; '
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('BookApplication.Web_db') AND session_id <> @@SPID;

EXEC sp_executesql @sql;