﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.Basics;

namespace Signum.Entities.SMS
{
    [Serializable]
    public class SMSConfigurationDN : EmbeddedEntity
    {
        CultureInfoDN defaultCulture;
        [NotNullValidator]
        public CultureInfoDN DefaultCulture
        {
            get { return defaultCulture; }
            set { Set(ref defaultCulture, value); }
        }
    }
}
