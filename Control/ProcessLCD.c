#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "Config.h"

void ProcessLCD(void)
{
	clearPosLCD(0, 13, 3);
	setCursorPosLCD(0, 13);
	writeStringLCD(DecompUnit_get_pumpstate() == ON ? "ON " : "OFF");

	clearPosLCD(0, 3, 6);
	setCursorPosLCD(0, 3);
	writeStringLCD(DecompUnit_get_valvestate() == OPEN ? "OPEN  " : "CLOSED");

	clearPosLCD(1, 0, 2);
	setCursorPosLCD(1, 0);
	writeStringLCD(config.controlledByPC ? "YS" : "NO");

	clearPosLCD(1, 3, 1);
	setCursorPosLCD(1, 3);
	writeStringLCD(config.connected ? "#" : "-");

	clearPosLCD(1, 5, 4);
	setCursorPosLCD(1, 5);
	writeIntegerLCD(DecompUnit_get_pressure(), DEC);

	clearPosLCD(1, 12, 4);
	setCursorPosLCD(1, 12);
	writeIntegerLCD(config.globalPressureBar * 1000, DEC);
}