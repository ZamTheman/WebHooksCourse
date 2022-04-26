#! /bin/bash

sqlcmd -S localhost,1433 -U sa -P pa55w0rd! -i sql/createfirstdb.sql
sqlcmd -S localhost,1433 -U sa -P pa55w0rd! -i sql/createseconddb.sql
