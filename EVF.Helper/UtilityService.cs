using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper
{

    /// <summary>
    /// The Utility Service Method.
    /// </summary>
    public static class UtilityService
    {
        /// <summary>
        /// Initial Error Result and Message to return.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ResultViewModel InitialResultError(string message)
        {
            return new ResultViewModel
            {
                IsError = true,
                Message = message
            };
        }
    }

}
