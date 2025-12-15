#include <Servo.h> // for servo
#include <math.h> // for kinematics

// base servo
Servo base_servo;

// linear servo
Servo rck_pinion;

// magnet pin
int mag = 2;

// lin actuator pins
int linPin1 = 4;
int linPin2 = 5;

// length variables
float base_theta = 0; // theta1
float L1 = 125;
float L2 = 2.16535;
float lin_L3; // L3
float L4 = (3.511811-0.275591);
float L5 = ((66.7+6.05)/25.4);
float rck_pin_L6; // magnet height control
float L7 = ((35+5+1.25)/25.4);
float L8 = 1.75;
float L9 = 0.5;

// coordinate variables
float px;
float py;
// float pz;

// Lets VS know if robot ready to recieve coordinates
enum RobotState {
  READY,
  BUSY
};

// set the robot state to ready
RobotState state = READY;

// shapeID, 0 = square, 1 = triangle
int shape;

void setup() {

  Serial.begin(9600);

  // Serial.print("\nArmbot Pos Code v5");
  
  // set the linear actuator pins to output
  pinMode(linPin1, OUTPUT);
  pinMode(linPin2, OUTPUT);

  // set electromagnet to output
  pinMode(mag, OUTPUT);
 
  // attach the base servo on pin 9 to the servo object
  base_servo.attach(9);

  // attach the rack and pinion servo on pin 10 to the servo object
  rck_pinion.attach(10);
  

  //////////// Reset/Home Positions ////////////
  ///// rack pinion (L6) /////
  rck_pinion.write(180);
  // Serial.print("\nRack pos is zeroed");

  delay(2000);

  ///// base servo (theta 1) /////
  // Goto base servo's 0°
  base_servo.write(base_theta);
  delay(2000);
  // Goto "true zero", which is 22.5°
  base_theta = 22.5;
  base_servo.write(base_theta);
  // Serial.print("\nBase Servo Zeroed\n");

  ///// linear actuator (L3) /////
  // Extend actuator
  digitalWrite(linPin1, LOW);
  digitalWrite(linPin2, HIGH);
  
  // 2000 msecs = 1 inch of travel
  delay(2000);

  // retract actuator
  digitalWrite(linPin1, HIGH);
  digitalWrite(linPin2, LOW);

  delay(2000);

  // turn off actuator
  digitalWrite(linPin1, LOW);
  digitalWrite(linPin2, LOW);

  // send ready signal to VS
  setState(READY);

}

void loop() {

  // get input from visual studio
  if (state == READY && Serial.available()) {

    // Discard anything until '<'
    while (Serial.available() && Serial.peek() != '<')
    {
      Serial.read();
    }

    if (!Serial.available())
      return;

    // Now we are guaranteed next char is '<'
    String line = Serial.readStringUntil('>');

    if (!line.startsWith("<"))
      return;

    // clear serial comms
    while (Serial.available())
        Serial.read();

    // remove '<'
    line.remove(0, 1); 
      
      // set c1 to be the first thing that comes from VS, c2 be the next, and c2+1 be the last (all comma seperated)
      int c1 = line.indexOf(',');
      int c2 = line.indexOf(',', c1 + 1);

      // convert from string to float
      px = line.substring(0, c1).toFloat();
      py = line.substring(c1 + 1, c2).toFloat();
      shape = line.substring(c2 + 1).toInt();


      // px is negative (on left side of base), add 180 degrees to base rotation value
      if(px<0){
      // define inverse kinematics
      base_theta = atan((py/px));
      base_theta = (base_theta)*(180/(3.14));
      base_theta = base_theta+180;
      // lin_L3 = (px*cos(base_theta))+(py*sin(base_theta))-L2-L4-L8;
      // lin_L3 = py - L2 - L4 - L8;
      }
      else{
      base_theta = atan((py/px));
      base_theta = (base_theta)*(180/(3.14));
      // lin_L3 = (px*cos(base_theta))+(py*sin(base_theta))-L2-L4-L8;
      // lin_L3 = py - L2 - L4 - L8;
      }

      lin_L3 = py - L2 - L4 - L8;

      
      // set state to busy so that VS does not send anything
      setState(BUSY);


      // goto shape
      base_rotation();
      delay(1000);
      L3_extension();
      delay(1000);
      L6_pick();
      delay(1000);
      L3_retract();
      delay(1000);

      // if recieved 1, its a triangle
      if (shape == 1)
      {
          base_rotation_back_Tri();

      }
      // if recieved 0, its a square
      else               
      {
          base_rotation_back_Sqre();
      }

      // drop shape and goto home pos
      delay(1000);
      L6_drop();
      delay(2000);
      base_rotation_back();
      delay(1000);

      // clear serial monitor
      Serial.flush();

      // tell VS bot is ready to receive next set of coordinates + shape
      setState(READY);

  }
  
  
}

void setState(RobotState newState)
{

  // let readdy = 1, busy = 0 when sent to VS

  if (state != newState)
  {
    state = newState;
    Serial.println(state == READY ? 1 : 0);
  }
}

// rotate to angle
void base_rotation(){
  // base_servo.write(130);
  // delay(3000);
  base_servo.write(base_theta);
  delay(1000);

  // base_servo.detach();
}

// if square, goto right
void base_rotation_back_Sqre(){

  base_servo.write(22.5);

  delay(1000);

}

// if triangle, goto left
void base_rotation_back_Tri(){

  base_servo.write(152.5);

  delay(1000);

}

// go home pos
void base_rotation_back(){

  base_servo.write(22.5);

  delay(1000);

}

// extend arm
void L3_extension(){
  // extend the linear actuator
  digitalWrite(linPin1, LOW);
  digitalWrite(linPin2, HIGH);
  

  // 2000 msecs = 1 inch of travel
  delay(2000*lin_L3);

  // stop motor
  digitalWrite(linPin1, LOW);
  digitalWrite(linPin2, LOW);

}

// retract arm
void L3_retract(){
  // retract the linear actuator
  digitalWrite(linPin1, HIGH);
  digitalWrite(linPin2, LOW);
  
  // 2000 msecs = 1 inch of travel
  delay(2000*lin_L3);
  // delay(2000);

  // stop motor
  digitalWrite(linPin1, LOW);
  digitalWrite(linPin2, LOW);
}

// pick shape
void L6_pick(){
   // put the magnet down
   rck_pinion.write(0);
   delay(2000);

  digitalWrite(mag, HIGH);

   delay(2000);

   // pull it back up
   rck_pinion.write(180);
}

// drop shape
void L6_drop(){
  // put the magnet down
   rck_pinion.write(10);
   delay(100);

  //  Serial.println("DROP");

  digitalWrite(mag, LOW);

   delay(1000);

  // digitalWrite(mag, LOW);

  //  delay(3000);

   // pull it back up
   rck_pinion.write(180);
}