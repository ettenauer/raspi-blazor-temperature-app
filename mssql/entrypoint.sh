#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -S localhost -l 60 -U SA -P P@ssword -i init.sql &
/opt/mssql/bin/sqlservr