import RPi.GPIO as GPIO
import time

BUTTON_PIN_1 = 16
BUTTON_PIN_2 = 21
LED_1_PIN = 19
LED_2_PIN = 26

duty_cycle_off = 0
duty_cycle_on = 99


GPIO.setmode(GPIO.BCM)

GPIO.setup(BUTTON_PIN_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)

GPIO.setup(LED_1_PIN, GPIO.OUT)
GPIO.setup(LED_2_PIN, GPIO.OUT)

LED_1_PWM = GPIO.PWM(LED_1_PIN, 100)
LED_2_PWM = GPIO.PWM(LED_2_PIN, 100)

LED_1_PWM.start(duty_cycle_off)
LED_2_PWM.start(duty_cycle_off)

previous_button_1_state = GPIO.input(BUTTON_PIN_1)
previous_button_2_state = GPIO.input(BUTTON_PIN_2)

try:
    while True:
        time.sleep(0.01)
        button_1_state = GPIO.input(BUTTON_PIN_1)
        button_2_state = GPIO.input(BUTTON_PIN_2)
        if button_1_state != previous_button_1_state:
            previous_button_1_state = button_1_state
            if button_1_state == GPIO.HIGH:
                print("Button 1 released and LED 1 off")
                LED_1_PWM.ChangeDutyCycle(duty_cycle_off)
            if button_1_state == GPIO.LOW:
                print("Button 1 pressed and LED 1 on")
                LED_1_PWM.ChangeDutyCycle(duty_cycle_on)
        if button_2_state != previous_button_2_state:
            previous_button_2_state = button_2_state
            if button_2_state == GPIO.HIGH:
                print("Button 2 released and LED 2 off")
                LED_2_PWM.ChangeDutyCycle(duty_cycle_off)
            if button_2_state == GPIO.LOW:
                print("Button 2 pressed and LED 2 on")
                LED_2_PWM.ChangeDutyCycle(duty_cycle_on)
except KeyboardInterrupt:
    LED_1_PWM.stop()
    LED_2_PWM.stop()
    GPIO.cleanup()
    print("GPIO stopped")
