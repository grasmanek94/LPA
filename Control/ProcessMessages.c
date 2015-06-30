#include <RP6ControlLib.h>
#include <LPA_lib_decompressie.h>

#include "Config.h"
#include "Message.h"

void ProcessMessages(void)
{
	if (Message_Receive())
	{
		if (!Message_IsCorrupt())
		{
			setStopwatch2(1);
			switch (Message_GetAction())
			{
			case CHECK_FOR_DECOMPRESSION_DEVICE:
			{
				byte readval = 0;
				if (Message_Read_byte(&readval))
				{
					if (readval == MS_DataBegin)
					{
						Message_BeginWrite();
						Message_SetAction(HERE_IS_A_DECOMPRESSION_DEVICE);
						Message_Write_byte(MS_DataEnd);
						Message_Send();

						config.connected = true;
					}
				}
			}
			break;

			case PING:
				if (config.connected)
				{
					Message_BeginWrite();
					Message_SetAction(PING);
					Message_Send();
				}
				break;

			case UPDATE_GLOBAL_PRESSURE:
				if (config.controlledByPC)
				{
					byte readval;
					if (Message_GetDataLenght() == 1 && Message_Read_byte(&readval))
					{
						config.globalPressureBar = readval;
					}
				}
				break;

			case PC_AQUIRE_CONTROL:
				if (config.connected)
				{
					config.controlledByPC = true;
					Message_BeginWrite();
					Message_SetAction(PC_AQUIRE_CONTROL_SUCCESS);
					Message_Send();
				}
				break;

			case PC_RELEASE_CONTROL:
				if (config.connected)
				{
					config.controlledByPC = false;
					Message_BeginWrite();
					Message_SetAction(PC_RELEASE_CONTROL_SUCCESS);
					Message_Send();
				}
				break;
			}
		}
	}

	if (config.connected)
	{
		if (getStopwatch1() > 100)
		{
			setStopwatch1(1);

			Message_BeginWrite();
			Message_SetAction(UPDATE_SETTINGS);
			Message_Write_byte(DecompUnit_get_valvestate());  //VentValveOpen;
			Message_Write_byte(DecompUnit_get_pumpstate());  //AirPumpOn;
			Message_Write_byte(config.globalPressureBar);  //PressureGlobalBar;
			Message_Write_byte(config.controlledByPC);  //ControlFromPC;
			Message_Write_Int16(DecompUnit_get_pressure()); //PressureMilliBar;
			Message_Send();
		}

		if (getStopwatch2() > 2500)
		{	//timeout occured
			config.controlledByPC = false;
			config.connected = false;
			setStopwatch2(1);
		}
	}
}