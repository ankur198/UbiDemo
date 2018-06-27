int currVal[] = { 0,0 };
int reqVal[] = { 0,0 };

void setup()
{
	for (int i = 0; i < 2; i++)
	{
		pinMode(i, OUTPUT);
	}
}

void doChange()
{
	for (int i = 0; i < 2; i++)
	{
		if (currVal[i] > reqVal[i])
		{
			currVal[i]--;
			analogWrite(i, currVal[i]);
		}
		else if (currVal[i] < reqVal[i])
		{
			currVal[i]++;
			analogWrite(i, currVal[i]);
		}
	}
}

void setPinAsync(int pin, int val)
{
	reqVal[pin] = val;
}

void transition()
{
	if (currVal[0] == reqVal[0])
	{
		if (currVal[0] == 0)
		{
			setPinAsync(0, 100);
		}
		else setPinAsync(0, 0);
	}

	if (currVal[1] == reqVal[1])
	{
		if (currVal[1] == 0)
		{
			setPinAsync(1, 255);
		}
		else setPinAsync(1, 0);
	}
}


void loop()
{

	doChange();

	delay(1);
}
