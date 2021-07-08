from pigpio_dht import DHT22
import time

gpio = 17 

sensor = DHT22(gpio)

while True:
    result = sensor.sample(samples=5)
    print(result)
    time.sleep(10)
