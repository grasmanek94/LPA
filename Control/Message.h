#ifndef MESSAGE_HEADER
#define MESSAGE_HEADER

#include <stdbool.h>
#include <stdint.h>

#include "../PressureControl/MsgStruct.cs"
#include "../PressureControl/Actions.cs" //predefined actions

typedef unsigned char byte;

void Message_Init(void);
byte Message_GetAction(void);
void Message_SetAction(byte action);
byte Message_GetDataLenght(void);
bool Message_Read_byte(byte* output);
bool Message_Write_byte(byte input);
bool Message_Read_Int16(int16_t* output);
bool Message_Write_Int16(int16_t input);
byte Message_Receive(void);//bytes gelezen
byte Message_Send(void);//bytes verstuurd
bool Message_IsCorrupt(void);
void Message_BeginWrite(void);//eerst dit, dan SetAction, dan Write_*_*, dan Send

#endif
