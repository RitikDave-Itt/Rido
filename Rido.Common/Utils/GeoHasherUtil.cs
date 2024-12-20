﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rido.Model.DataTypes;

using Geohash;

namespace Rido.Common.Utils
{
    public static class GeoHasherUtil
    {
        public static string Encoder(LocationType location)
        {
            var geohasher = new Geohasher();

            return geohasher.Encode(Convert.ToDouble(location.Latitude), Convert.ToDouble(location.Longitude));

        }
    }
}
