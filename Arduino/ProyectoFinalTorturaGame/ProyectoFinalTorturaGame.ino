int trigger = 4;//salida
int echo = 3;//entrada
int tiempo = 0;
float distancia = 0; 
float distanciaAnterior = 0; 
 
 
int SW_pin = 2; // digital pin connected to switch output
int X_pin = 0; // analog pin connected to X output
int Y_pin = 1; // analog pin connected to Y output
bool paraPrimeraVez = true;

bool ultrasonic = false;
int cambioultrasonic = 0;


float analogicH_valor = 0;
float analogicM_valor = 0;
 
void setup() {
  pinMode(SW_pin, INPUT);
  digitalWrite(SW_pin, HIGH);

  pinMode(trigger,OUTPUT);
  pinMode(echo,INPUT);
  digitalWrite(trigger,LOW);
  Serial.begin(9600);

}

void loop() {
  //JOYSTICK
    int Press = digitalRead(SW_pin);
    int X = analogRead(X_pin);
    int Y = analogRead(Y_pin);
    Serial.print(String(Press)+"#");
    //"X-axis
    Serial.print(String(X)+"#");
    //Y-axis
    Serial.print(String(Y)+"#");

    
  //ULTRASONICO
  digitalWrite(trigger,HIGH);
  delayMicroseconds(5);
  digitalWrite(trigger,LOW);
  
  tiempo = pulseIn(echo,HIGH);
  distancia = 0.0173*tiempo;

  //Serial.println(distancia);
  if(distancia < 15 && distancia>0)
  {
    Serial.print("SMASH#");
  }
  else
  {
    Serial.print("NADA#");
    }
   

  //POTENCIOMETRO
  analogicH_valor = map(analogRead(A3),0,1023,0,180);
  analogicM_valor= map(analogRead(A4),0,1023,0,180);
  Serial.print(String(analogicH_valor)+"#");
  Serial.println(analogicM_valor);
  


}
