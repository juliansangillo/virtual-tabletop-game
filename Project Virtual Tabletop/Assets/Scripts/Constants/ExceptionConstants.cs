namespace NaughtyBikerGames.ProjectVirtualTabletop.Constants {
	public struct ExceptionConstants {
                public const string VA_ARGUMENT_NULL = "A required argument was null.";
                public const string VA_SPACE_INVALID = "The space is invalid. Please verify neither the row or column are negative and try again.\nParameter name = {0}";
                public const string VA_SPACE_INVALID_DEFAULT = "The space is invalid. Please verify neither the row or column are negative and try again.";
                public const string VA_SPACE_OUT_OF_BOUNDS = "The space doesn't exist on map.";
                public const string VA_ELEMENT_EXISTS_ON_SPACE = "The space is not empty. This space must be unoccupied.\nParameter name = {0}";
                public const string VA_ELEMENT_DOESNT_EXIST_ON_SPACE = "The space is empty. An element must exist on this space.\nParameter name = {0}";
                public const string VA_NUMBER_OF_ROWS_INVALID = "Grid details are invalid. Number of rows must be greater than zero. NumberOfRows = {0}";
                public const string VA_NUMBER_OF_COLUMNS_INVALID = "Grid details are invalid. Number of columns must be greater than zero. NumberOfColumns = {0}";
                public const string VA_LIST_OF_TOKENS_INVALID = "Grid details are invalid. List of tokens must not be null.";
	}
}