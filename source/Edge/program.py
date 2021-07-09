from pigpio_dht import DHT22
from azure.storage.fileshare import ShareFileClient
import time
import sys
import os
from datetime import datetime
import csv

ACCOUNT_NAME = os.environ['ACCOUNT_NAME']
ACCOUNT_KEY = os.environ['ACCOUNT_KEY']
DATA_PIN = 17
DEVICE_ID = os.environ['DEVICE_ID']
SHARE = os.environ['SHARE']

connection_string = "DefaultEndpointsProtocol=https;AccountName={};AccountKey={};EndpointSuffix=core.windows.net".format(ACCOUNT_NAME, ACCOUNT_KEY)
sensor = DHT22(DATA_PIN, timeout_secs=2)
 
while True:
    try: 
        rowCount = 0
        filename = DEVICE_ID + "_" + datetime.utcnow().strftime("%Y-%M-%d-%H%M%S")
        with open(filename, 'w', newline='') as file:
            writer = csv.writer(file, delimiter=';')
            writer.writerow(["Date", "DegreeCelsius", "DeviceId"])
            while rowCount < 10:
                try:
                    data = sensor.sample(samples=15)
                    print(f"write measurement to local file {filename}")
                    writer.writerow([datetime.utcnow().isoformat(), data["temp_c"], DEVICE_ID])
                    rowCount = rowCount + 1
                except Exception as ex:
                    print(ex)

        file_client = ShareFileClient.from_connection_string(conn_str=connection_string, share_name=SHARE, file_path=filename)

        with open(filename, "rb") as source_file:
            print(f"upload {filename} measurement to azure storage")
            file_client.upload_file(source_file)

        try:
            os.remove(filename)
        except OSError as e:
            print(f"Error during file delete: {e.filename}") 

    except Exception as ex:
        print(ex)
    
