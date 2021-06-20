FROM microsoft/mssql-server-linux:latest

COPY ["mssql/init.sql", "init.sql"]
COPY ["mssql/entrypoint.sh", "entrypoint.sh"]

#RUN chmod +x entrypoint.sh

CMD /bin/bash ./entrypoint.sh