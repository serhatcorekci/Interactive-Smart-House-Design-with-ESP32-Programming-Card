#include <ESP32Servo.h>

#define SERIAL_BAUD_RATE 115200
///////////////////////////////////////////////////////////////////////////////////////////////////
const int PIRSENSOR = 19;           // SENSOR PIN connected to GPIO19
int pirpinStateCurrent = LOW;       // current state of pin

// L298N Motor Driver Pins
#define ENA 15                      // Hız kontrolü için PWM pin
#define IN1 2                       // Motor yön kontrolü için IN1 pin
#define IN2 0                       // Motor yön kontrolü için IN1 pin
const int ledPirPin = 21;           // Siren ledi için pin
///////////////////////////////////////////////////////////////////////////////////////////////////
const int RAINSENSOR = 32;          // SENSOR PIN connected to GPIO34
int rain_analog_value;              // analog value from the rain sensor

#define SERVO_PIN  26               // SENSOR PIN connected to GPIO26
Servo servoMotor;                   // create servo object to control a servo
int servoPosition = 0;              // Servo motorun başlangıç konumu (varsayılan olarak 90 derece)
///////////////////////////////////////////////////////////////////////////////////////////////////
const int TRIG_PIN = 23;            // GPIO13 pin connected to TRIG pin of HCSR04 sensor
const int ECHO_PIN = 22;            // GPIO12 pin connected to ECHO pin of HCSR04 sensor
long duration_us, distance_cm;
///////////////////////////////////////////////////////////////////////////////////////////////////
#define LIGHT_SENSOR_PIN 36         // GPIO36 pin connected to LIGHT_SENSOR_PIN (ADC0)
int ldr_analog_value;
const int ledOdaPin = 4;           // Oda ledi için pin
///////////////////////////////////////////////////////////////////////////////////////////////////
#define LIGHT_SENSOR_PIN1 39        // GPIO36 pin connected to LIGHT_SENSOR_PIN (ADC0)
int ldr_analog_value1;
const int ledOdaPin1 = 5;          // Oda1 ledi için pin
///////////////////////////////////////////////////////////////////////////////////////////////////
#define LIGHT_SENSOR_PIN2 34        // GPIO36 pin connected to LIGHT_SENSOR_PIN (ADC0)
int ldr_analog_value2;
const int ledOdaPin2 = 12;          // Oda2 ledi için pin
///////////////////////////////////////////////////////////////////////////////////////////////////
#define LIGHT_SENSOR_PIN3 35        // GPIO36 pin connected to LIGHT_SENSOR_PIN (ADC0)
int ldr_analog_value3;
const int ledOdaPin3 = 13;          // Oda3 ledi için pin
///////////////////////////////////////////////////////////////////////////////////////////////////
void setup() {
  Serial.begin(SERIAL_BAUD_RATE);   // initialize serial communication
  pinMode(PIRSENSOR, INPUT);
  pinMode(RAINSENSOR, INPUT);
  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);
  servoMotor.attach(SERVO_PIN);
  servoMotor.write(servoPosition);

  pinMode(ENA, OUTPUT);
  pinMode(IN1, OUTPUT);
  pinMode(IN2, OUTPUT);
  pinMode(ledPirPin, OUTPUT);
  pinMode(ledOdaPin, OUTPUT);
  pinMode(ledOdaPin1, OUTPUT);
  pinMode(ledOdaPin2, OUTPUT);
  pinMode(ledOdaPin3, OUTPUT);
  
  digitalWrite(IN1, LOW);
  digitalWrite(IN2, LOW);
}

void loop() {
// MOTION SENSOR___________________________________________
  pirpinStateCurrent = digitalRead(PIRSENSOR);
    Serial.print(pirpinStateCurrent);
    Serial.print("/");
// MOTION SENSOR END_______________________________________

// RAIN SENSOR_____________________________________________
  rain_analog_value = analogRead(RAINSENSOR);
    Serial.print(rain_analog_value);
    Serial.print("/");
// RAIN SENSOR END_________________________________________

// DISTANCE SENSOR_________________________________________
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);

  // measure duration of pulse from ECHO pin
  duration_us = pulseIn(ECHO_PIN, HIGH);

  // calculate the distance
  distance_cm = (duration_us/2) / 29.1;

    Serial.print(distance_cm);
    Serial.print("/");
// DISTANCE SENSOR END_____________________________________

// LDR SENSOR______________________________________________
  ldr_analog_value = analogRead(LIGHT_SENSOR_PIN);
    Serial.print(ldr_analog_value);
    Serial.print("/");
// LDR SENSOR END__________________________________________

// LDR SENSOR1_____________________________________________
  ldr_analog_value1 = analogRead(LIGHT_SENSOR_PIN1);
    Serial.print(ldr_analog_value1);
    Serial.print("/");
// LDR SENSOR1 END_________________________________________

// LDR SENSOR2_____________________________________________
  ldr_analog_value2 = analogRead(LIGHT_SENSOR_PIN2);
    Serial.print(ldr_analog_value2);
    Serial.print("/");
// LDR SENSOR2 END_________________________________________

// LDR SENSOR3_____________________________________________
  ldr_analog_value3 = analogRead(LIGHT_SENSOR_PIN3);
    Serial.println(ldr_analog_value3);
// LDR SENSOR3 END_________________________________________

if (Serial.available()) {
    char command = Serial.read();

    if (command == 'a') {
      // Servo motoru 0 konumuna getir
      servoMotor.write(0);
      
    } else if (command == 'b') {
      // Servo motoru 90 konumuna getir
      servoMotor.write(90);
      
    } else if (command == 'c') {
      // DC motoru durdur
      digitalWrite(IN1, LOW);
      digitalWrite(IN2, LOW);
      analogWrite(ENA, 0);  // Hızı sıfıra ayarla
      digitalWrite(ledPirPin,LOW);
      
    } else if (command == 'd') {
      // DC motoru yavaş döndür
      analogWrite(ENA, 150);  // Hız değerini ayarlayabilirsin
      digitalWrite(IN1, HIGH);
      digitalWrite(IN2, LOW);
      digitalWrite(ledPirPin,HIGH);
      
    } else if (command == 'e') {
      // DC motoru hızlı döndür
      analogWrite(ENA, 255);  // Hız değerini ayarlayabilirsin
      digitalWrite(IN1, HIGH);
      digitalWrite(IN2, LOW);
      digitalWrite(ledPirPin,HIGH);
      
    } else if (command == 'f') {
      // Fotoğraf çek
      
      
    } else if (command == 'h') {
      // Oda1'in ışığını kapat
      analogWrite(ledOdaPin, 0);
      
    } else if (command == 'i') {
      // Oda1'in ışığı low
      analogWrite(ledOdaPin, 85);
      
    } else if (command == 'j') {
      // Oda1'in ışığı medium
      analogWrite(ledOdaPin, 170);
      
    } else if (command == 'k') {
      // Oda1'in ışığı high
      analogWrite(ledOdaPin, 255);
      
    } else if (command == 'l') {
      // Oda2'nin ışığını kapat
      analogWrite(ledOdaPin1, 0);
      
    } else if (command == 'm') {
      // Oda2'nin ışığı low
      analogWrite(ledOdaPin1, 85);
      
    } else if (command == 'n') {
      // Oda2'nin ışığı medium
      analogWrite(ledOdaPin1, 170);
      
    } else if (command == 'o') {
      // Oda2'nin ışığı high
      analogWrite(ledOdaPin1, 255);
      
    } else if (command == 'p') {
      // Oda3'ün ışığını kapat
      analogWrite(ledOdaPin2, 0);
      
    } else if (command == 'r') {
      // Oda3'ün ışığı low
      analogWrite(ledOdaPin2, 85);
      
    } else if (command == 's') {
      // Oda3'ün ışığı medium
      analogWrite(ledOdaPin2, 170);
      
    } else if (command == 't') {
      // Oda3'ün ışığı high
      analogWrite(ledOdaPin2, 255);

    } else if (command == 'u') {
      // Oda4'ün ışığını kapat
      analogWrite(ledOdaPin3, 0);
      
    } else if (command == 'v') {
      // Oda4'ün ışığı low
      analogWrite(ledOdaPin3, 85);
      
    } else if (command == 'y') {
      // Oda4'ün ışığı medium
      analogWrite(ledOdaPin3, 170);
      
    } else if (command == 'z') {
      // Oda4'ün ışığı high
      analogWrite(ledOdaPin3, 255);
    }
  }
}
