IF DB_ID('MYDB') IS NOT NULL
BEGIN
	PRINT 'Dropping DB'
	DROP DATABASE MYDB
END

CREATE DATABASE MYDB
PRINT 'DB MYDB created'