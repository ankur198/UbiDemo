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

String Verify(String req)
{
	int d = req.indexOf("y");
	d = req[d + 1];
	d = d * 123;
	return String(d);
}

String PinManager::ProcessResponse(String req)
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
	else if (req.indexOf("status") != -1)
	{
		Serial.println("status");
		return Status();
	}
	else if (req.indexOf("verify") != -1)
	{
		Serial.println("verify");
		return Verify(req);
	}
}
