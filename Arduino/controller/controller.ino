int i = 0;

void setup() {
  pinMode(13, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  Serial.println(i); 
  i = (i + 1) % 5;
  /* digitalWrite(LED_BUILTIN, HIGH); */
  /* delay(1000); */
  digitalWrite(LED_BUILTIN, LOW);
  delay(1000);
}
