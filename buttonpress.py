import RPi.GPIO as GPIO
import time
import argparse
from pythonosc import udp_client

BUTTON_PIN_1 = 16 #play button
BUTTON_PIN_2 = 21 #record button
BUTTON_PIN_3 = 18 #state 1 button
BUTTON_PIN_4 = 23 #state 2 button
BUTTON_PIN_5 = 24 #state 3 button
LED_1_PIN = 19 #record button light
LED_2_PIN = 26 #state 1 button light
LED_3_PIN =  25 #state 2 button light
LED_4_PIN =  22 #state 3 button light

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

LED_1_PWM = GPIO.PWM(LED_1_PIN, 100)

LED_1_PWM.start(duty_cycle_on)

previous_button_1_state = GPIO.input(BUTTON_PIN_1)
previous_button_2_state = GPIO.input(BUTTON_PIN_2)

led_2_state=0
led_3_state=0
led_4_state=0
button_3_state=1
button_4_state=1
button_5_state=1
button_3_state_old=0
button_4_state_old=0
button_5_state_old=0

GPIO.output(LED_2_PIN, led_2_state)
GPIO.output(LED_3_PIN, led_3_state)
GPIO.output(LED_4_PIN, led_4_state)

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
                client.send_message("/playButton", 1)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_on)
                print("Button 1 released and LED 1 on")
            if button_1_state == GPIO.LOW:
                client.send_message("/playButton", 0)
                client.send_message("/recordButtonPress", 1)
                LED_1_PWM.ChangeDutyCycle(duty_cycle_off)
                print("Button 1 pressed, LED 1 off and play stopped")

        #play recording
        if button_1_state == GPIO.HIGH:
            print("Record button is not being pressed")
            if button_2_state != previous_button_2_state:
                previous_button_2_state = button_2_state
                if button_2_state == GPIO.HIGH:
                    client.send_message("/playButton", 0)
                    print("Button 2 released")
                if button_2_state == GPIO.LOW:
                    client.send_message("/playButton", 1)
                    print("Button 2 pressed")

        #toggle on audio effect for cumulus
        if button_3_state==0 and button_3_state_old==1:
             led_2_state= not led_2_state
             GPIO.output(LED_2_PIN, led_2_state)
             print("State 1 is ",led_2_state)
             if led_3_state==1:
                 led_3_state= not led_3_state
                 GPIO.output(LED_3_PIN, led_3_state)
                 print("State 2 is ",led_3_state)
             button_4_state=button_4_state_old
             if led_4_state==1:
                 led_4_state= not led_4_state
                 GPIO.output(LED_4_PIN, led_4_state)
                 print("State 3 is ",led_4_state)
             button_5_state_old=button_5_state
        button_3_state_old=button_3_state

        #toggle on audio effect for stratus
        if button_4_state==0 and button_4_state_old==1:
             led_3_state= not led_3_state
             GPIO.output(LED_3_PIN, led_3_state)
             print("State 2 is ",led_3_state)
             if led_4_state==1:
                 led_4_state= not led_4_state
                 GPIO.output(LED_4_PIN, led_4_state)
                 print("State 3 is ",led_4_state)
             button_5_state=button_5_state_old
             if led_2_state==1:
                 led_2_state= not led_2_state
                 GPIO.output(LED_2_PIN, led_2_state)
                 print("State 1 is ",led_2_state)
             button_3_state=button_3_state_old
        button_4_state_old=button_4_state

        #toggle on audio effect for cirrus
        if button_5_state==0 and button_5_state_old==1:
             led_4_state= not led_4_state
             GPIO.output(LED_4_PIN, led_4_state)
             print("State 3 is ",led_4_state)
             if led_2_state==1:
                 led_2_state= not led_2_state
                 GPIO.output(LED_2_PIN, led_2_state)
                 print("State 1 is ",led_2_state)
             button_3_state=button_3_state_old
             if led_3_state==1:
                 led_3_state= not led_3_state
                 GPIO.output(LED_3_PIN, led_3_state)
                 print("State 2 is ",led_3_state)
             button_4_state=button_4_state_old
        button_5_state_old=button_5_state

except KeyboardInterrupt:
    LED_1_PWM.stop()
    GPIO.cleanup()
    print("GPIO stopped")
