using System;

namespace ProjectVirtualTabletop.Exceptions {
	public class InvalidSpaceException : Exception {
		private const string defaultMessage = "Space is invalid. Please verify neither the row or column are negative and try again.";

		public InvalidSpaceException() : base(defaultMessage) {}
		public InvalidSpaceException(string message) : base(message) {}
	}
}