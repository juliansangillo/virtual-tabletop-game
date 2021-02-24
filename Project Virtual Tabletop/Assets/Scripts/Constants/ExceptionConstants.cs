namespace ProjectVirtualTabletop.Constants {
	public struct ExceptionConstants {
                public const string VA_ARGUMENT_NULL = "A required argument was null.";
                public const string VA_SPACE_INVALID = "The space is invalid. Please verify neither the row or column are negative and try again.\nParameter name = {0}";
                public const string VA_SPACE_INVALID_DEFAULT = "The space is invalid. Please verify neither the row or column are negative and try again.";
                public const string VA_SPACE_OUT_OF_BOUNDS = "The space doesn't exist on map.";
                public const string VA_ELEMENT_EXISTS_ON_SPACE = "The space is not empty. This space must be unoccupied.\nParameter name = {0}";
                public const string VA_ELEMENT_DOESNT_EXIST_ON_SPACE = "The space is empty. An element must exist on this space.\nParameter name = {0}";
	}
}