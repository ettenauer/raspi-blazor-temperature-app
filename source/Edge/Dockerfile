# For more information, please refer to https://aka.ms/vscode-docker-python
FROM python:3.7

# Keeps Python from generating .pyc files in the container
ENV PYTHONDONTWRITEBYTECODE=1

# Turns off buffering for easier container logging
ENV PYTHONUNBUFFERED=1

RUN apt-get update

# Install pip requirements
COPY ["source/Edge/requirements.txt", "requirements.txt"]
RUN xargs -a requirements.txt apt-get -y install

#RUN git clone https://github.com/joan2937/pigpio \
# && cd pigpio \
# && make install prefix=/tmp/pigpio CFLAGS=-DEMBEDDED_IN_VM

RUN apt install pigpio

WORKDIR /app
COPY ["source/Edge/","/app"]

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-python-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# start daemon for GPIO
RUN systemctl start pigpiod

# During debugging, this entry point will be overridden. For more information, please refer to https://aka.ms/vscode-docker-python-debug
CMD ["python", "program.py"]
