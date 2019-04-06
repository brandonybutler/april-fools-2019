namespace _1April2019.Payload
{
    /**
     * <summary>
     * Declares all of the core functions of a payload.
     * </summary>
     **/
    public interface IPayload
    {
        /**
         * <summary>
         * This will return true or false depending on whether the payload is currently running.
         * </summary>
         **/
        bool IsRunning();
        /**
         * <summary>
         * A public function which can be accessed from any class to forcedly terminate the payload and any of its threads.
         * </summary>
         **/
        void StopPayload();
    }
}