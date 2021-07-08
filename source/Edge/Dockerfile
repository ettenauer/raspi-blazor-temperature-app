# For more information, please refer to https://aka.ms/vscode-docker-python
FROM debian

# Keeps Python from generating .pyc files in the container
ENV PYTHONDONTWRITEBYTECODE=1

# Turns off buffering for easier container logging
ENV PYTHONUNBUFFERED=1

RUN apt-get update
RUN apt-get install -y python3
RUN apt-get install -y python3-pip
RUN apt-get install -y python3-setuptools

# Install pip requirements
COPY ["source/Edge/requirements.txt", "requirements.txt"]
RUN pip3 install -r requirements.txt

WORKDIR /app
COPY ["source/Edge/","/app"]

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-python-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# During debugging, this entry point will be overridden. For more information, please refer to https://aka.ms/vscode-docker-python-debug
CMD pigpiod; python3 program.py
