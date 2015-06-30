#if defined(ARDUINO)
#include "Arduino.h"
int getBufferLength()
{
	return Serial.available();
}

int readChar()
{
	return Serial.read();
}

void writeChar(byte b)
{
	Serial.write(b);
}
#else
#include <RP6ControlLib.h>
#endif

#include "Message.h"

#include <string.h>

typedef struct
{
	byte messageBuffer[MS_BufferSize];
	byte offset;
	byte localCorruptionCheck;
	bool corrupt;
} SMessage;

SMessage * message;

#define VarDataLength (message->messageBuffer[MS_DataLen])
#define VarAction (message->messageBuffer[MS_Action])
#define VarCorrChk (message->messageBuffer[MS_CorrCheck])
#define VarOffset (message->offset)
#define VarBuffer (message->messageBuffer)
#define CurrCorrChk (message->localCorruptionCheck)
#define VarIsCorrupt (message->corrupt)

byte CalculateCorruptionCheck(void)
{
	long sum = 0;
	if (VarDataLength > 0)
	{
		for (int i = 0; i < VarDataLength; ++i)
		{
			sum += VarBuffer[i + MS_DataBegin];
		}
		sum %= MS_CorrModulo;
		sum ^= VarDataLength;
	}
	sum += VarAction;

	return (byte)sum;
}

void Message_BeginWrite(void)
{
	VarOffset = 0;
	VarDataLength = 0;
	VarAction = 0;
}

void Message_Init(void)
{
	message = malloc(sizeof(SMessage));

	VarBuffer[MS_StartA] = MS_CharA;
	VarBuffer[MS_StartB] = MS_CharB;
	VarBuffer[MS_AlwaysZero] = 0;

	Message_BeginWrite();
}

byte Message_GetAction(void)
{
	return message->messageBuffer[MS_Action];
}

void Message_SetAction(byte action)
{
	message->messageBuffer[MS_Action] = action;
}

byte Message_GetDataLenght(void)
{
	return VarDataLength;
}

bool Message_Read_byte(byte* output)
{
	byte size = sizeof(byte);
	if (VarOffset + size > VarDataLength)
	{
		return false;
	}
	*output = VarBuffer[MS_DataBegin + VarOffset];
	VarOffset += size;
	return true;
}

bool Message_Write_byte(byte input)
{
	byte size = sizeof(byte);
	if (VarOffset + size > MS_MaxDataLen)
	{
		return false;
	}
	VarBuffer[MS_DataBegin + VarOffset] = input;
	VarOffset += size;
	if (VarOffset > VarDataLength)
	{
		VarDataLength = VarOffset;
	}
	return true;
}

bool Message_Read_Int16(int16_t* output)
{
	byte size = sizeof(int16_t);
	if (VarOffset + size > VarDataLength)
	{
		return false;
	}
	//this gives warnings but is completely valid:
	//*output = *((int16_t*)& (VarBuffer[MS_DataBegin + VarOffset]));
	//*output = VarBuffer[MS_DataBegin + VarOffset] | VarBuffer[MS_DataBegin + VarOffset + 1] << 8;
	//or
	//*output = VarBuffer[MS_DataBegin + VarOffset] << 8 | VarBuffer[MS_DataBegin + VarOffset + 1];
	//or
	memcpy(output, &VarBuffer[MS_DataBegin + VarOffset], size);
	VarOffset += size;
	return true;
}

bool Message_Write_Int16(int16_t input)
{
	byte size = sizeof(int16_t);
	if (VarOffset + size > MS_MaxDataLen)
	{
		return false;
	}
	memcpy(&VarBuffer[MS_DataBegin + VarOffset], &input, size);
	VarOffset += size;
	if (VarOffset > VarDataLength)
	{
		VarDataLength = VarOffset;
	}
	return true;
}

byte Message_Receive(void)
{
	if (getBufferLength() < MS_DataBegin)
	{
		return 0;
	}

	if (readChar() != MS_CharA)
	{
		return 0;
	}

	if (readChar() != MS_CharB)
	{
		return 0;
	}

	Message_BeginWrite();

	VarAction = readChar();
	VarDataLength = readChar();

	if (VarDataLength > MS_MaxDataLen)
	{
		VarDataLength = MS_MaxDataLen;
	}

	VarCorrChk = readChar();

	if (getBufferLength() < VarDataLength)
	{
		//sleep 25 ms (well or less..)
		//delay(25);
		mSleep(25);
		if (getBufferLength() < VarDataLength)
		{
			VarIsCorrupt = true;
			return MS_DataBegin;//discart message because we are not receiving anything further
		}
	}

	for (int i = 0; i < VarDataLength; ++i)
	{
		VarBuffer[MS_DataBegin + i] = readChar();
	}

	CurrCorrChk = CalculateCorruptionCheck();
	VarIsCorrupt = VarCorrChk != CurrCorrChk;
	return MS_DataBegin + VarDataLength;
}

byte Message_Send(void)
{
	int top = MS_DataBegin + VarDataLength;

	VarCorrChk = CalculateCorruptionCheck();

	for (int i = 0; i < top; ++i)
	{
		writeChar(VarBuffer[i]);
	}
	return MS_DataBegin + VarDataLength;
}

bool Message_IsCorrupt(void)
{
	return VarIsCorrupt;
}
