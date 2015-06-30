#ifndef CONFIG_HEADER
#define CONFIG_HEADER

#include "Message.h"

typedef struct
{
	bool connected;
	byte globalPressureBar;
	bool controlledByPC;
} SConfig;

extern SConfig config;
#endif