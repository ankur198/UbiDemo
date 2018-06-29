#include "PinManager.h"
#include <ESP8266WiFi.h>;

char *userName = "Ankur";
char *pwd = "youCANhack1";

WiFiServer server(80);
PinManager pins;


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

void setup()
{
	Serial.begin(9600);
	ConnectToWifi();
	StartServer();
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
		if (timeout == 300)
		{
			return;
		}
	}
	//started recieving data
	String req = client.readStringUntil('\r');
	Serial.println(req);
	client.flush();	
	req = pins.ProcessRequest(req);
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
