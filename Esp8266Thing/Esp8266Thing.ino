#include <ESP8266WiFi.h>;

char *userName = "Ankur";
char *pwd = "youCANhack1";

WiFiServer server(80);

int pins[] = { 16,5,4,0,2,14,12,13,15 };
int vals[] = { 0,0,0,0,0,0,0,0,0 };


void ConnectToWifi()
{
	Serial.println("Attemting to connect");
	WiFi.mode(WIFI_STA);
	WiFi.begin(userName, pwd);

	while (WiFi.status() != WL_CONNECTED)
	{
		delay(500);
		Serial.print('.');
	}
	Serial.println("Connected to wifi");
	Serial.print("IP: ");
	Serial.println(WiFi.localIP());
}

void StartServer()
{
	server.begin();
	Serial.println("Server started");
}

void InitializePin()
{
	for (uint i = 0; i < 9; i++)
	{
		pinMode(pins[i], OUTPUT);
	}
}

void setup()
{
	Serial.begin(9600);
	ConnectToWifi();
	StartServer();
	InitializePin();
}


void SetPinVal(int pin, int pinValue)
{
	Serial.println(String(pin) + ":" + String(pinValue));
	for (int i = 0; i < 9; i++)
	{
		if (pins[i] == pin)
		{
			vals[i] = pinValue;
			analogWrite(pin, pinValue);
			return;
		}
	}
	Serial.println("no pin found");
}

String SetPin(String req)
{
	String raw = req.substring(req.indexOf('/') + 1);
	raw = raw.substring(raw.indexOf('/') + 1);
	Serial.println(raw);
	String pin = String(raw[0]);
	if (raw[1] != '/')
	{
		pin += String(raw[1]);
		raw = raw.substring(3);
	}
	else raw = raw.substring(2);
	String pinValue = String();
	for (uint i = 0; i < raw.indexOf('H') - 1; i++)
	{
		pinValue += String(raw[i]);
	}
	Serial.println(raw);
	Serial.println(pin.toInt());
	Serial.println(pinValue.toInt());
	SetPinVal(pin.toInt(), pinValue.toInt());
	return "OK";

}

int GetVal(int pin)
{
	for (int i = 0; i < 9; i++)
	{
		if (pin == pins[i])
		{
			return vals[i];
		}
	}
	return 0;
}

String GetPin(String req)
{
	String raw = req.substring(req.indexOf('/') + 1);
	raw = raw.substring(raw.indexOf('/') + 1);
	String pin = String(raw[0]);
	if (raw[1] != '/')
	{
		pin += String(raw[1]);
	}
	Serial.println(raw);
	Serial.println(pin.toInt());
	int val = GetVal(pin.toInt());
	return String(val);
}

String ProcessResponse(String req)
{
	if (req.indexOf("setpin") != -1)
	{
		Serial.println("Going to set pin");
		return SetPin(req);
	}
	else if (req.indexOf("getpin") != -1)
	{
		Serial.println("Going to get pin");
		return GetPin(req);
	}
}

void GotClient(WiFiClient client)
{
	int timeout = 0;
	//wait to send data
	Serial.println("new client");
	while (!client.available())
	{
		delay(1);
		timeout++;
		if (timeout==300)
		{
			return;
		}
	}
	//started recieving data
	String req = client.readStringUntil('\r');
	Serial.println(req);
	client.flush();
	req = ProcessResponse(req);
	Serial.println(req);
	String s = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n" + req;
	client.print(s);
	Serial.println("Client disconnected");
}


void CheckForClient()
{
	WiFiClient client = server.available();
	if (!client)
	{
		return;
		//no client
	}
	GotClient(client);

}


void loop()
{
	CheckForClient();
	delay(20);
}
