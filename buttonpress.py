import RPi.GPIO as GPIO
import time
import argparse
import random
from pythonosc import udp_client


BUTTON_PIN_1 = 16
BUTTON_PIN_2 = 21
LED_1_PIN = 19
#LED_2_PIN = 26
#TRIG_PIN = 23 Pin setup ultrasonic sensor
#ECHO_PIN = 24 " "

duty_cycle_off = 0
duty_cycle_on = 99


GPIO.setmode(GPIO.BCM)

GPIO.setup(BUTTON_PIN_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)

GPIO.setup(LED_1_PIN, GPIO.OUT)
#GPIO.setup(LED_2_PIN, GPIO.OUT)

#GPIO.setup(TRIG_PIN, GPIO.OUT) GPIO setup for ultrasonic sensor
#GPIO.setup(ECHO_PIN, GPIO.IN) " "

LED_1_PWM = GPIO.PWM(LED_1_PIN, 100)
#LED_2_PWM = GPIO.PWM(LED_2_PIN, 100)

LED_1_PWM.start(duty_cycle_on)
#LED_2_PWM.start(duty_cycle_off)

previous_button_1_state = GPIO.input(BUTTON_PIN_1)
previous_button_2_state = GPIO.input(BUTTON_PIN_2)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--ip", default="192.168.1.11",
      help="The ip of the OSC server")
    parser.add_argument("--port", type=int, default=57120,
      help="The port the OSC server is listening on")
    args = parser.parse_args()

    client = udp_client.SimpleUDPClient(args.ip, args.port)

try:
    while True:
        '''
        Code for an ultrasonic sensor

        GPIO.output(TRIG_PIN, 0)
        time.sleep(2E-6)
        GPIO.output(TRIG_PIN, 1)
        time.sleep(10E-6)
        GPIO.output(TRIG_PIN, 0)
        while GPIO.input(ECHO_PIN)== 0:
            pass
        echoStartTime = time.time()
        while GPIO.input(ECHO_PIN) == 1:
            pass
        echoStopTime = time.time()
        pingTravelTime = echoStopTime - echoStartTime
        distance = 767*pingTravelTime*5280*12/3600
        distance_to_target = distance/2
        print(round(distance_to_target,1)," inches")
        time.sleep(0.1)
        '''
        time.sleep(0.01)
        button_1_state = GPIO.input(BUTTON_PIN_1)
        button_2_state = GPIO.input(BUTTON_PIN_2)
        if button_1_state != previous_button_1_state:
            previous_button_1_state = button_1_state
            if button_1_state == GPIO.HIGH:
                client.send_message("/recordButtonRelease", 0)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Button 1 released and LED 1 on")
            if button_1_state == GPIO.LOW:
                client.send_message("/recordButtonPress", 1)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Button 1 pressed and LED 1 off")
        if button_2_state != previous_button_2_state:
            previous_button_2_state = button_2_state
            if button_2_state == GPIO.HIGH:
                client.send_message("/playButtonPress", 0)
                print("Button 2 released")
                #LED_2_PWM.ChangeDutyCycle(duty_cycle_off)
            if button_2_state == GPIO.LOW:
                client.send_message("/playButtonPress", 1)
                print("Button 2 pressed")
                #LED_2_PWM.ChangeDutyCycle(duty_cycle_on)
except KeyboardInterrupt:
    LED_1_PWM.stop()
    #LED_2_PWM.stop()
    GPIO.cleanup()
    print("GPIO stopped")
