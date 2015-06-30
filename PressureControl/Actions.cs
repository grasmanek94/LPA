#if !__LINE__
namespace PressureControl
{
    public
#endif
    enum Actions
    {
        PING = 1,
        CHECK_FOR_DECOMPRESSION_DEVICE,
        HERE_IS_A_DECOMPRESSION_DEVICE,
        TIMEOUT_OCCURED,
        UPDATE_SETTINGS,
        PC_AQUIRE_CONTROL,
        PC_RELEASE_CONTROL,
        INCORRECT_PRESSURE,
        CORRECT_PRESSURE,
        UPDATE_GLOBAL_PRESSURE,
        CONNECTION_CORRUPTED,
        PC_AQUIRE_CONTROL_SUCCESS,
        PC_RELEASE_CONTROL_SUCCESS
    };
#if !__LINE__
}
#endif
