#include <RP6ControlLib.h>

#include "Config.h"

void ProcessKeys(void)
{
	if (!config.controlledByPC)
	{
		uint8_t key = getPressedKeyNumber();
		if (key > 0 && key < 5)
		{
			config.globalPressureBar = 2 * (key - 1);
		}
	}
}