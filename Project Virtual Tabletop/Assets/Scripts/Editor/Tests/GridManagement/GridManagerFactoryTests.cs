using System;
using System.Collections.Generic;
using NUnit.Framework;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Factories;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.GridManagement {
	public class GridManagerFactoryTests {
        GridManagerFactory gridManagerFactory;

        [SetUp]
        public void SetUp() {
            gridManagerFactory = new GridManagerFactory();
        }

        [TestCase(5,3,0,0)]
        [TestCase(10,10,9,9)]
		public void CreateGridManager_GivenGridDetailsWithOneToken_ReturnACorrectGridManager(int numRows, int numColumns, int row1, int col1) {
            Token token1 = new Token(new GridSpace(row1, col1));
			GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);

            GridManager actual = gridManagerFactory.CreateGridManager(gridDetails);

            Assert.AreEqual(numRows, actual.Grid.GetLength(AppConstants.ROW_DIMENSION));
            Assert.AreEqual(numColumns, actual.Grid.GetLength(AppConstants.COLUMN_DIMENSION));
            Assert.AreEqual(token1, actual.Grid[row1, col1]);
		}

		[TestCase(5,3,0,0,4,0,4,2)]
		public void CreateGridManager_GivenGridDetailsWithThreeTokens_ReturnACorrectGridManager(int numRows, int numColumns, int row1, int col1, 
        int row2, int col2, int row3, int col3) {
            Token token1 = new Token(new GridSpace(row1, col1));
            Token token2 = new Token(new GridSpace(row2, col2));
            Token token3 = new Token(new GridSpace(row3, col3));
			GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);
            gridDetails.Tokens.Add(token2);
            gridDetails.Tokens.Add(token3);

            GridManager actual = gridManagerFactory.CreateGridManager(gridDetails);

            Assert.AreEqual(numRows, actual.Grid.GetLength(AppConstants.ROW_DIMENSION));
            Assert.AreEqual(numColumns, actual.Grid.GetLength(AppConstants.COLUMN_DIMENSION));
            Assert.AreEqual(token1, actual.Grid[row1, col1]);
            Assert.AreEqual(token2, actual.Grid[row2, col2]);
            Assert.AreEqual(token3, actual.Grid[row3, col3]);
		}

        [TestCase(0)]
        [TestCase(-1)]
        public void CreateGridManager_GivenInvalidGridDetailsWhereNumberOfRowsAreZeroOrNegative_ThrowArgumentException(int numRows) {
            Token token1 = new Token(new GridSpace(0, 0));
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = 1;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);

            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, numRows), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManagerFactory.CreateGridManager(gridDetails);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CreateGridManager_GivenInvalidGridDetailsWhereNumberOfColumnsAreZeroOrNegative_ThrowArgumentException(int numColumns) {
            Token token1 = new Token(new GridSpace(0, 0));
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 1;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);

            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, numColumns), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManagerFactory.CreateGridManager(gridDetails);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void CreateGridManager_GivenInvalidGridDetailsWhereListOfTokensIsNull_ThrowArgumentException() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 1;
            gridDetails.NumberOfColumns = 1;
            gridDetails.Tokens = null;

            Exception expected = new ArgumentException(ExceptionConstants.VA_LIST_OF_TOKENS_INVALID, "gridDetails");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManagerFactory.CreateGridManager(gridDetails);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void CreateGridManager_GivenNullGridDetails_ThrowArgumentNullException() {
            Exception expected = new ArgumentNullException("gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                gridManagerFactory.CreateGridManager(null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }
	}
}
