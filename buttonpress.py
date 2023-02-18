import RPi.GPIO as GPIO
import time
import argparse
from pythonosc import udp_client

BUTTON_PIN_1 = 16 #play button
BUTTON_PIN_2 = 21 #record button
BUTTON_PIN_3 = 18 #cumulus
BUTTON_PIN_4 = 23 #cirrus
BUTTON_PIN_5 = 24 #nimbus
LED_1_PIN = 19 #record button light
LED_2_PIN = 6 #play button light
LED_3_PIN = 26 #cumulus
LED_4_PIN =  25 #cirrus
LED_5_PIN =  22 #nimbus

duty_cycle_off = 0
duty_cycle_on = 99

GPIO.setmode(GPIO.BCM)

GPIO.setup(BUTTON_PIN_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_3, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_4, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_5, GPIO.IN, pull_up_down=GPIO.PUD_UP)

GPIO.setup(LED_1_PIN, GPIO.OUT)
GPIO.setup(LED_2_PIN, GPIO.OUT)
GPIO.setup(LED_3_PIN, GPIO.OUT)
GPIO.setup(LED_4_PIN, GPIO.OUT)
GPIO.setup(LED_5_PIN, GPIO.OUT)

LED_1_PWM = GPIO.PWM(LED_1_PIN, 100)
LED_2_PWM = GPIO.PWM(LED_2_PIN, 100)

LED_1_PWM.start(duty_cycle_on)
LED_2_PWM.start(duty_cycle_on)

previous_button_1_state = GPIO.input(BUTTON_PIN_1)
previous_button_2_state = GPIO.input(BUTTON_PIN_2)

led_3_state=1 #cumulus state led
led_4_state=0 #cirrus state led
led_5_state=0 #nimbus state led
button_3_state=1 #cumulus state button
button_4_state=1 #cirrus state button
button_5_state=1 #nimbus state button
button_3_state_old=0
button_4_state_old=0
button_5_state_old=0

GPIO.output(LED_3_PIN, led_3_state)
GPIO.output(LED_4_PIN, led_4_state)
GPIO.output(LED_5_PIN, led_5_state)

trans_dict = {'True':'on','False':'off'}

#OSC communication
if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--ip", default="192.168.1.6", #"10.60.8.58", == ACCD IP
      help="The ip of the OSC server")
    parser.add_argument("--port", type=int, default=57120,
      help="The port the OSC server is listening on")
    args = parser.parse_args()

    client = udp_client.SimpleUDPClient(args.ip, args.port)

if button_3_state == 1 and led_3_state == 1:
    print("Cumulus state is ", led_3_state)

try:
    while True:
        time.sleep(0.01)

        button_1_state = GPIO.input(BUTTON_PIN_1)
        button_2_state = GPIO.input(BUTTON_PIN_2)
        button_3_state = GPIO.input(BUTTON_PIN_3)
        button_4_state = GPIO.input(BUTTON_PIN_4)
        button_5_state = GPIO.input(BUTTON_PIN_5)

        #record microphone
        if button_1_state != previous_button_1_state:
            previous_button_1_state = button_1_state
            if button_1_state == GPIO.HIGH:
                client.send_message("/recordButtonRelease", 0)
                #client.send_message("/playButton", 1)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Record button released and record light on")
            if button_1_state == GPIO.LOW:
                #client.send_message("/playButton", 0)
                client.send_message("/recordButtonPress", 1)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Record button pressed and record light off")

        #play recording with cumulus audio effect
        if button_2_state != previous_button_2_state:
            previous_button_2_state = button_2_state
            if button_2_state == GPIO.HIGH and led_3_state == 1:
                client.send_message("/playButtonCumulus", 0)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Play button for cumulus state released and light on")
            if button_2_state == GPIO.LOW and led_3_state == 1:
                client.send_message("/playButtonCumulus", 1)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Play button for cumulus state pressed and light off")

        #play recording with cirrus audio effect
        if button_2_state != previous_button_2_state:
            previous_button_2_state = button_2_state
            if button_2_state == GPIO.HIGH and led_4_state == 1:
                client.send_message("/playButtonCirrus", 0)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Play button for cirrus state released and light on")
            if button_2_state == GPIO.LOW and led_4_state == 1:
                client.send_message("/playButtonCirrus", 1)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Play button for cirrus state pressed")

        #play recording with nimbus audio effect
        if button_2_state != previous_button_2_state:
            previous_button_2_state = button_2_state
            if button_2_state == GPIO.HIGH and led_5_state == 1:
                client.send_message("/playButtonNimbus", 0)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Play button for nimbus state released")
            if button_2_state == GPIO.LOW and led_5_state == 1:
                client.send_message("/playButtonNimbus", 1)
                LED_2_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Play button for nimbus state pressed")

        #toggle on audio effect for cumulus
        if button_3_state==0 and button_3_state_old==1:
             led_3_state= not led_3_state
             GPIO.output(LED_3_PIN, led_3_state)
             print("Cumulus state is ",led_3_state)
             if led_4_state==1:
                 led_4_state= not led_4_state
                 GPIO.output(LED_4_PIN, led_4_state)
                 print("Cirrus state is ",led_4_state)
             button_4_state=button_4_state_old
             if led_5_state==1:
                 led_5_state= not led_5_state
                 GPIO.output(LED_5_PIN, led_5_state)
                 print("Nimbus state is ",led_5_state)
             button_5_state_old=button_5_state
        button_3_state_old=button_3_state

        #toggle on audio effect for cirrus
        if button_4_state==0 and button_4_state_old==1:
             led_4_state= not led_4_state
             GPIO.output(LED_4_PIN, led_4_state)
             print("Cirrus state is ",led_4_state)
             if led_5_state==1:
                 led_5_state= not led_5_state
                 GPIO.output(LED_5_PIN, led_5_state)
                 print("Nimbus state is ",led_5_state)
             button_5_state=button_5_state_old
             if led_3_state==1:
                 led_3_state= not led_3_state
                 GPIO.output(LED_3_PIN, led_3_state)
                 print("Cumulus state is ",led_3_state)
             button_3_state=button_3_state_old
        button_4_state_old=button_4_state

        #toggle on audio effect for nimbus
        if button_5_state==0 and button_5_state_old==1:
             led_5_state= not led_5_state
             GPIO.output(LED_5_PIN, led_5_state)
             print("Nimbus state is ",led_5_state)
             if led_3_state==1:
                 led_3_state= not led_3_state
                 GPIO.output(LED_3_PIN, led_3_state)
                 print("Cumulus state is ",led_3_state)
             button_3_state=button_3_state_old
             if led_4_state==1:
                 led_4_state= not led_4_state
                 GPIO.output(LED_4_PIN, led_4_state)
                 print("Cirrus state is ",led_4_state)
             button_4_state=button_4_state_old
        button_5_state_old=button_5_state

except KeyboardInterrupt:
    LED_1_PWM.stop()
    GPIO.cleanup()
    print("GPIO stopped")
