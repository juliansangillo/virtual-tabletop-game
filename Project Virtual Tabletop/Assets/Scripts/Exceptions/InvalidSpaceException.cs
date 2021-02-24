using System;
using ProjectVirtualTabletop.Constants;

namespace ProjectVirtualTabletop.Exceptions {
	public class InvalidSpaceException : Exception {
		public InvalidSpaceException() : base(ExceptionConstants.VA_SPACE_INVALID_DEFAULT) {}
		public InvalidSpaceException(string message) : base(message) {}
	}
}