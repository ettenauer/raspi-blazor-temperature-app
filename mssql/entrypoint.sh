#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -S localhost -l 60 -U SA -P $1 -Q 'CREATE LOGIN appuser WITH PASSWORD="'$2'"' &
/opt/mssql-tools/bin/sqlcmd -S localhost -l 60 -U SA -P $1 -i init.sql &
/opt/mssql/bin/sqlservr