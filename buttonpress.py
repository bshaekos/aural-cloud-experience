import RPi.GPIO as GPIO
import time

BUTTON_PIN_1 = 16
BUTTON_PIN_2 = 21
LED_1_PIN = 19
LED_2_PIN = 26
TRIG_PIN = 23
ECHO_PIN = 24

duty_cycle_off = 0
duty_cycle_on = 99


GPIO.setmode(GPIO.BCM)

GPIO.setup(BUTTON_PIN_1, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(BUTTON_PIN_2, GPIO.IN, pull_up_down=GPIO.PUD_UP)

GPIO.setup(LED_1_PIN, GPIO.OUT)
GPIO.setup(LED_2_PIN, GPIO.OUT)

GPIO.setup(TRIG_PIN, GPIO.OUT)
GPIO.setup(ECHO_PIN, GPIO.IN)

LED_1_PWM = GPIO.PWM(LED_1_PIN, 100)
LED_2_PWM = GPIO.PWM(LED_2_PIN, 100)

LED_1_PWM.start(duty_cycle_off)
LED_2_PWM.start(duty_cycle_off)

previous_button_1_state = GPIO.input(BUTTON_PIN_1)
previous_button_2_state = GPIO.input(BUTTON_PIN_2)

try:
    while True:
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
