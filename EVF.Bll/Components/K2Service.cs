using EVF.Bll.Components.InterfaceComponents;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Components
{
    public class K2Service : IK2Service
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="K2Service" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public K2Service(IConfigSetting config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]



        #endregion

    }
}
