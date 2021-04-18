using System;
using NaughtyBikerGames.ProjectVirtualTabletop.Enums;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;
using NUnit.Framework;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.Utilities {
	public class ConvertDistanceTests {
        [Test]
        public void From_GivenADistanceUnitAndAValueInThatUnit_ReturnAConvertDistanceObjectWithThoseValues() {
            ConvertDistance actual = ConvertDistance.From(Distance.METERS, 1d);

            Assert.AreEqual(Distance.METERS, actual.Unit);
            Assert.AreEqual(1d, actual.Value);
        }

        [Test]
        public void To_FromADistanceInMetersToMeters_ReturnSameDistanceAsNothingChanges() {
            double expected = 1.524d;
            
            double actual = ConvertDistance.From(Distance.METERS, expected).To(Distance.METERS);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void To_FromADistanceInFeetToMeters_ReturnDistanceInMeters() {
            double expected = 1.524d;
            
            double actual = ConvertDistance.From(Distance.FEET, 5.0d).To(Distance.METERS);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void To_FromADistanceInMetersToFeet_ReturnDistanceInFeet() {
            double expected = 5.0d;
            
            double actual = ConvertDistance.From(Distance.METERS, 1.524d).To(Distance.FEET);

            Assert.AreEqual(expected, Math.Round(actual));
        }
	}
}