using System;
using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Enums;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Utilities {
	public class ConvertDistance {
        private static IDictionary<Tuple<Distance, Distance>, double> conversionTable = new Dictionary<Tuple<Distance, Distance>, double> {
            { new Tuple<Distance, Distance>(Distance.METERS, Distance.FEET), 3.28084d },
            { new Tuple<Distance, Distance>(Distance.FEET, Distance.METERS), 0.3048d }
        };

        public Distance Unit { get; private set; }
        public double Value { get; private set; }

        public static ConvertDistance From(Distance unit, double value) {
            return new ConvertDistance(unit, value);
        }

        public double To(Distance newUnit) {
            double result = Value;
            if(newUnit != Unit)
                result = Value * conversionTable[new Tuple<Distance, Distance>(Unit, newUnit)];

            return result;
        }

		private ConvertDistance(Distance unit, double value) {
			this.Unit = unit;
			this.Value = value;
		}
	}
}