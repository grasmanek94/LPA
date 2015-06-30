#if !__LINE__
namespace PressureControl
{
    public
#endif
    enum MsgStruct
    {
            MS_CharA               = 0xE6,
            MS_CharB               = 0xC9,

            MS_StartA              = 0x00,
            MS_StartB              = 0x01,
            MS_Action              = 0x02,
            MS_DataLen             = 0x03,
            MS_CorrCheck           = 0x04,
            MS_DataBegin           = 0x05,

            MS_DataEnd             = 0x1E,
            MS_AlwaysZero          = 0x1F,
            MS_MaxDataLen          = 0x1A,
            MS_BufferSize          = 0x20,

            MS_CorrModulo          = 0x100
    };
#if !__LINE__
}
#endif
