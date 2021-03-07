using System;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Exceptions {
	public class InvalidSpaceException : Exception {
		public InvalidSpaceException() : base(ExceptionConstants.VA_SPACE_INVALID_DEFAULT) {}
		public InvalidSpaceException(string message) : base(message) {}
	}
}