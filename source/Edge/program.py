from pigpio_dht import DHT22

gpio = 17 

sensor = DHT22(gpio)

result = sensor.read()
print(result)
