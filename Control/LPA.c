#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "VSIntelliSenseFix.h"

#include "Message.h"
#include "Config.h"

#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "ProcessLCD.h"
#include "ProcessMessages.h"
#include "ProcessKeys.h"
#include "ProcessLeds.h"
#include "ProcessPresurarization.h"

void setup(void)
{
	initRP6Control();
	initLCD();
	setLEDs(0b0000);

	//VS CLOSED AP OFF
	//YS 50000 / 60000
	showScreenLCD("VS        AP    ", "          /     ");

	DecompUnit_reset();

	Message_Init();

	startStopwatch1();//sender
	startStopwatch2();//timeout counter

	config.controlledByPC = false;
	config.globalPressureBar = 0;
}

void loop(void)
{
	ProcessKeys();
	ProcessLeds();
	ProcessMessages();
	ProcessPresurarization();
	ProcessLCD();
}

int main(void)
{
	setup();
	while (true)
	{
		loop();
	}
	return 0;
}
