#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "Config.h"

void ProcessLeds(void)
{
	int pressurization = DecompUnit_get_pressure() / 10;
	int otherpresurization = config.globalPressureBar * 100;
	if (otherpresurization - 5 < pressurization && pressurization < otherpresurization + 5)
	{
		setLEDs(1 << (config.globalPressureBar / 2));
	}
	else
	{
		setLEDs(0b0000);
	}
}