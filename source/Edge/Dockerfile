# For more information, please refer to https://aka.ms/vscode-docker-python
FROM python:3.7

# Keeps Python from generating .pyc files in the container
ENV PYTHONDONTWRITEBYTECODE=1

# Turns off buffering for easier container logging
ENV PYTHONUNBUFFERED=1

# Install pip requirements
COPY ["source/Edge/requirements.txt", "requirements.txt"]
RUN python -m pip install -r requirements.txt

RUN apt update
RUN apt install -y git

WORKDIR /app
COPY ["source/Edge/","/app"]

RUN git clone https://github.com/adafruit/Adafruit_Python_DHT.git
RUN python3 Adafruit_Python_DHT/setup.py install

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-python-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# During debugging, this entry point will be overridden. For more information, please refer to https://aka.ms/vscode-docker-python-debug
CMD ["python", "program.py"]
