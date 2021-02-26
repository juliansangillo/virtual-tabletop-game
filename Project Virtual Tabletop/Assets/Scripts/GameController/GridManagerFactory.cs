using System;
using ProjectVirtualTabletop.Constants;
using ProjectVirtualTabletop.Entities;

namespace ProjectVirtualTabletop.GameController {
	public class GridManagerFactory {
		public GridManager CreateGridManager(GridDetails gridDetails) {
            ThrowExceptionIfArgumentIsNull(gridDetails);
            ThrowExceptionIfArgumentIsInvalid(gridDetails);

			Element[,] grid = new Element[gridDetails.NumberOfRows, gridDetails.NumberOfColumns];

			foreach (Token token in gridDetails.Tokens) {
                grid[token.CurrentSpace.Row, token.CurrentSpace.Column] = token;
			}

			return new GridManager(grid);
		}

        private void ThrowExceptionIfArgumentIsNull(GridDetails gridDetails) {
            if(gridDetails == null)
                throw new ArgumentNullException("gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);
        }

        private void ThrowExceptionIfArgumentIsInvalid(GridDetails gridDetails) {
            if(!gridDetails.IsNumberOfRowsValid())
                throw new ArgumentException(
                    string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, gridDetails.NumberOfRows),
                    "gridDetails"
                );
            else if(!gridDetails.IsNumberOfColumnsValid())
                throw new ArgumentException(
                    string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, gridDetails.NumberOfColumns), 
                    "gridDetails"
                );
            else if(!gridDetails.IsTokensValid())
                throw new ArgumentException(ExceptionConstants.VA_LIST_OF_TOKENS_INVALID, "gridDetails");
        }
	}
}