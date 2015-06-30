#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "Config.h"

void ProcessPresurarization(void)
{
	unsigned int maximumDeviation = 30;
	unsigned int wantedPressure = config.globalPressureBar * 1000;
	unsigned int currentPressure = DecompUnit_get_pressure();

	if (currentPressure > wantedPressure + maximumDeviation)
	{
		DecompUnit_set_pump(OFF);
		DecompUnit_set_valve(OPEN);
	}
	else if (currentPressure < (wantedPressure == 0 ? wantedPressure : wantedPressure - maximumDeviation))
	{
		DecompUnit_set_pump(ON);
		DecompUnit_set_valve(CLOSED);
	}
	else
	{
		DecompUnit_set_pump(OFF);
		DecompUnit_set_valve(CLOSED);
	}
}
