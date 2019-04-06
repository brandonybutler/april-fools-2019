using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using _1April2019.Payload;

namespace _1April2019.Resource
{
    #region Checkpoint list
    /**
     * <summary>
     * A list of all available payloads.
     * </summary>
     **/
    public enum Checkpoint
    {
        REVERSING_SCREEN,
        SECURITY_ALERT
    }
    #endregion

    #region Payload activator
    /**
     * <summary>
     * Options for payload activation.
     * </summary>
     **/
    public class PayloadPointer
    {
        /**
         * <summary>
         * Activate the specified payload.
         * </summary>
         **/
        public static void ActivatePayload(Checkpoint payload)
        {
            switch (payload)
            {
                case Checkpoint.REVERSING_SCREEN:
                    // Instantiate a new instance of the ReversingScreen class, and set running to true.
                    #pragma warning disable IDE0059
                    ReversingScreen reversingScreen = new ReversingScreen(true);
                    #pragma warning restore IDE0059
                    break;
                case Checkpoint.SECURITY_ALERT:
                    // Instantiate a new instance of the SecurityAlert class, and show the dialog.
                    SecurityAlert securityAlert = new SecurityAlert();
                    securityAlert.ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}