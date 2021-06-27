FROM microsoft/mssql-server-linux:latest

COPY ["mssql/init.sql", "init.sql"]
COPY ["mssql/entrypoint.sh", "entrypoint.sh"]

CMD /bin/bash ./entrypoint.sh ${SA_PASSWORD} ${APP_USER_PASSWORD}