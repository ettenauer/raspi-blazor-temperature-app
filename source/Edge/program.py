import os
import pigpio
import DHT22

os.chgdir('pigpio_dht22')
pi = pigpio.pi()
s = DHT22.sensor(pi, 17)
s.trigger()

print('{:3.2f}'.format(s.temperature()/1.))
