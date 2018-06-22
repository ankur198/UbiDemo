
int ledPin = 11;    // LED connected to digital pin 11
					// pin 1 for esp8266

void setup()
{
	pinMode(ledPin, OUTPUT);
}

void turnOn();
void turnOff();
void turnOnSmooth();
void turnOffSmooth();

void loop()
{
	turnOn();
	delay(1000);
	turnOff();
	delay(1000);
}

int maxVal = 255;
int del = 50;

void turnOn()
{
	digitalWrite(ledPin, HIGH);
}

void turnOff()
{
	digitalWrite(ledPin, LOW);
}

void turnOnSmooth()
{
	for (int i = 0; i <= maxVal; i++)
	{
		analogWrite(ledPin, i);
		delay(del);
	}
}

void turnOffSmooth()
{
	for (int i = maxVal; i >= 0; i--)
	{
		analogWrite(ledPin, i);
		delay(del);
	}
}
