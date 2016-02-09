void setup() 
{
   Serial.begin(115200);
}

void loop() 
{
   Serial.write(255);
   Serial.write(0);//database identification number
   Serial.write(1);//x1 400
   Serial.write(1);//x2
   Serial.write(1);//y1 500
   Serial.write(1);//y2
   Serial.write(analogRead(A10) / 4);
   Serial.write(analogRead(A11) / 4);
   Serial.write(analogRead(A12) / 4);
   Serial.write(analogRead(A13) / 4);
   Serial.write(analogRead(A14) / 4);
   Serial.write(analogRead(A15) / 4);
/*
   Serial.print(analogRead(A10) / 4);
   Serial.print(" ");
   Serial.print(analogRead(A11) / 4);
   Serial.print(" ");
   Serial.print(analogRead(A12) / 4);
   Serial.print(" ");
   Serial.print(analogRead(A13) / 4);
   Serial.print(" ");
   Serial.print(analogRead(A14) / 4);
   Serial.print(" ");
   Serial.println(analogRead(A15) / 4);
   */
}
