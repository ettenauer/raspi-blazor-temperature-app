FROM mcr.microsoft.com/mssql/server:2019-latest

COPY ["mssql/init.sql", "init.sql"]
COPY ["mssql/entrypoint.sh", "entrypoint.sh"]

CMD /bin/bash ./entrypoint.sh ${SA_PASSWORD} ${APP_USER_PASSWORD}