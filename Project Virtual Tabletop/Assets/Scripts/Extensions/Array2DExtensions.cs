using NaughtyBikerGames.ProjectVirtualTabletop.Constants;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Extensions {
	public static class Array2DExtensions {
        public static T[] AsFlat<T>(this T[,] array) {
            int width = array.GetLength(AppConstants.COLUMN_DIMENSION);
            int height = array.GetLength(AppConstants.ROW_DIMENSION);
            T[] result = new T[width * height];

            int index = 0;
            for (int row = 0; row < height; row++)
                for (int col = 0; col < width; col++) {
                    result[index] = array[row, col];
                    index++;
                }

            return result;
        }
	}
}