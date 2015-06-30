/*****************************************************************************
* Decompression Unit control of LPA box
* author:  Corne Govers
* version: 23-12-2010
*****************************************************************************/

#ifndef LPA_LIB_DECOMPRESSIE_H
#define LPA_LIB_DECOMPRESSIE_H

/******************************************************************************
* Pump which drives the pressure in the decompression chamber is either OFF or ON
* Valve which releases the pressure is either CLOSED or OPEN
* Note: Pressure is to be measured in mbar relative to air pressure. 
*       So if pressure = 0 then pressure in chamber equals outside pressure.
*/ 
typedef enum {OFF, ON} PumpState;
typedef enum {CLOSED, OPEN} ValveState;


/******************************************************************************
* function DecompUnit_reset initializes the simulator on RP6 control hardware
* pre:  no pre-conditions needed
* post: actual pressure = 0 AND pumpstate = OFF (Green LED on) AND 
*       valve = OPEN AND RGB-Led is RED
*/
void DecompUnit_reset( void );

/******************************************************************************
* function DecompUnit_get_pressure detects the actual pressure in the unit
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: actual pressure in mbar. 0 <= pressure <= MAX_PRESSURE
*/
unsigned int DecompUnit_get_pressure( void );

/******************************************************************************
* function DecompUnit_get_maxpressure returns the maximum pressure the pump can deliver.
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: MAX_PRESSURE (in mbar)
*/
unsigned int DecompUnit_get_maxpressure( void );

/******************************************************************************
* function DecompUnit_set_pump switches the pump ON or OFF
* When the pump is on, the pressure increment due to the pump is proportional to the  
*      pressure difference between maximum achievable (pm) and the unit (p): dp/dt = k.(pm - p).
*      k = 0.1 
* pre:  function DecompUnit_reset() has been called at least once
* post: Pump power is switched ON or OFF, depending on argument
*       If p = ON then RED led on LPA-box turns on. Pressure starts rising.
*       If p = OFF then it takes short while to be really off/ During this time
*                  RED and GREEN led on LPA-box are on.
*       If p = OFF and pump has really stopped then GREEN led on LPA-box is on.
* Note: After switching the pump ON, the pressure increment/sec increases up to a maximum.
*       After switching the pump OFF, the pressure increment/sec decreases to zero.
*       Calling DecompUnit_set_pump(ON) for second, third etc time has no effect. 
*       Idem with argument (OFF)
*/
void DecompUnit_set_pump( PumpState p );

/******************************************************************************
* function DecompUnit_get_pumpstate returns the pumpstate
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: actual pump state (either ON or OFF)
*/
PumpState DecompUnit_get_pumpstate( void );

/******************************************************************************
* function DecompUnit_set_valve opens or closes the valve.
* When the valve is open, the pressure decreases proportional to the pressure 
*      difference between chamber (p) and air (p0): dp/dt = k.(p - p0). 
*      k = 0.1 
* pre:  function DecompUnit_reset() has been called at least once.
* post: Valve is OPEN or CLOSED, depending on argument
*       If OPEN then RGB led on LPA-box is GREEN.
*       If CLOSED then RGB led on LPA-box is RED.
*/
void DecompUnit_set_valve( ValveState s );

/******************************************************************************
* function DecompUnit_get_valvestate returns the valve state
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: actual valve state (either OPEN or CLOSED)
*/
ValveState DecompUnit_get_valvestate( void );

/******************************************************************************
* function DecompUnit_get_pumpCapacity returns the capacity of the pump
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: pump capacity (in mbar/sec)
*/
unsigned int DecompUnit_get_pumpCapacity( void );


/******************************************************************************
* function DecompUnit_get_valveLoss returns the actual loss via the valve
* pre:  function DecompUnit_reset() has been called at least once
* post: -
* return: actual loss capacity (in mbar/sec)
*/
unsigned int DecompUnit_get_valveLoss( void );

#endif
