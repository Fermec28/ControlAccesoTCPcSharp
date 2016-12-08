// LED on pin 12
    int led = 7;
    
    // Incoming serial data
    int data=0;
    
    void setup() {                
      // Pin 12 set to OUTPUT
      pinMode(led, OUTPUT);
      
      // Start listening on the serialport
      Serial.begin(9600);
    }
    
    void loop() {
      
      if(Serial.available()>0){
          
          // Read from serialport
          data = Serial.read();      
    
          // Check and see if data received == 4
          if(data=='4') {   
            // Blink the LED 3 times
            for(int i=0;i<3;i++){
                digitalWrite(led, HIGH);
                delay(1000);
                digitalWrite(led,LOW);
                delay(1000);
            }
        
           // Reset data to 0
           data=0;
         }
      }
    }

