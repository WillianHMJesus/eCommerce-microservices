#!/bin/bash
set -e

wait_time=90s 
password=Passw0rd

echo importing data will start in $wait_time...
sleep $wait_time

echo running StartupScript...
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -i StartupScript.sql

exec "$@"