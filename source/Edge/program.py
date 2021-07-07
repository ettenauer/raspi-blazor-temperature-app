import board
import adafruit_dht

dhtDevice = adafruit_dht.DHT22(board.D7)

print("Temp: {:.1f} C ".format(dhtDevice.temperature))