#include <ESP8266WiFi.h>;

char *userName = "Ankur";
char *pwd = "youCANhack1";

WiFiServer server(80);

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

void SendResponse(WiFiClient client, String response)
{
	String s = response;
	client.print(s[]);
	Serial.println("Client disconnected");
}

void GotClient(WiFiClient client)
{
	//wait to send data
	Serial.println("new client");
	while (!client.available())
	{
		delay(1);
	}
	//started recieving data
	String req = client.readStringUntil('\r');
	Serial.println(req);
	client.flush();
	SendResponse(client, req);
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
}
