// 
// 
// 

#include<Arduino.h>
#include "PinManager.h"

const int pins[9] = { 16,5,4,0,2,14,12,13,15 };
int vals[9];

PinManager::PinManager()
{
	for (int i = 0; i < 9; i++)
	{
		vals[i] = 0;
	}
}

void ChangePinSmoothly(int index, int value)
{
	int currentVal = vals[index];
	if (currentVal > value)
	{
		for (int i = currentVal - 1; i >= value; i--)
		{
			analogWrite(pins[index], i);
			delay(5);
		}
	}
	else
	{
		for (int i = value; i < currentVal; i++)
		{
			analogWrite(pins[index], i);
			delay(5);
		}
	}
}

void SetPinVal(int pin, int pinValue)
{
	Serial.println(String(pin) + ":" + String(pinValue));
	if (pinValue > 1023 || pinValue < 0)
	{
		Serial.println("Invalid pinValue");
		return;
	}
	for (int i = 0; i < 9; i++)
	{
		if (pins[i] == pin)
		{
			ChangePinSmoothly(i, pinValue);
			vals[i] = pinValue;
			return;
		}
	}
	Serial.println("no pin found");
}

String SetPin(String req)
{
	String raw = req;
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
	String raw = req;
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

String Status()
{
	String r = "[";
	for (int i = 0; i < 9; i++)
	{
		r += vals[i];
		if (i != 8)
		{
			r += ",";
		}
		else
		{
			r += "]";
		}
	}
	return r;
}

String VerifySelf(String req)
{
	Serial.println(req);
	int d = req[0];
	d = d * 123;
	return String(d);
}

bool VerifySender(String req)
{
	Serial.println(req);
	int key = req[0];
	req = req.substring(2);
	Serial.println(req);

	String val = "";
	for (int i = 0; i < req.indexOf("/"); i++)
	{
		val += req[i];
	}
	Serial.println(val);
	if (key * 123 == val.toInt())
	{
		Serial.println("Sender verified");
		return true;
	}
	Serial.println("Sender Not verified");
	return false;
}

String PinManager::ProcessRequest(String req)
{

	req = req.substring(req.indexOf("/") + 1);
	if (VerifySender(req))
	{
		req = req.substring(req.indexOf("/") + 1);
		req = req.substring(req.indexOf("/") + 1);
		Serial.println(req);

		if (req.indexOf("setpin") != -1)
		{
			req = req.substring(req.indexOf("/") + 1);
			Serial.println("Going to set pin");
			return SetPin(req);
		}
		else if (req.indexOf("getpin") != -1)
		{
			req = req.substring(req.indexOf("/") + 1);
			Serial.println("Going to get pin");
			return GetPin(req);
		}
		else if (req.indexOf("status") != -1)
		{
			req = req.substring(req.indexOf("/") + 1);
			Serial.println("status");
			return Status();
		}
		else if (req.indexOf("verify") != -1)
		{
			req = req.substring(req.indexOf("/") + 1);
			Serial.println("verify");
			return VerifySelf(req);
		}
	}
	return "Invalid request";
}
