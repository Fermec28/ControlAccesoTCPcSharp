String readString;

void setup() {
  // initialize digital pin 13 as an output.
  pinMode(13, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  char incomingByte;
  while (Serial.available() > 0) {    
    delay(5);    
    incomingByte = Serial.read();
    readString +=incomingByte;
  }
  if ( readString == "joseferon") {

    digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
                              // wait for a second    
  }  
  else if(readString == "joseferoff"){
     digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
                               // wait for a second    
    }
 readString="";  
}
