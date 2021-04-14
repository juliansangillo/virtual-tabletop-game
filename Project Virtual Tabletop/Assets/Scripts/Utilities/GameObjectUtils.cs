using System;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using UnityEngine;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Utilities {
	public static class GameObjectUtils {
        public static GameObject FindGameObjectFromGridSpace(GridSpace space) {
            ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfSpaceIsInvalid(space, "space", ExceptionConstants.VA_SPACE_INVALID);

            return GameObject.FindGameObjectsWithTag(AppConstants.GRID_SPACE_TAG)
                .Where(o => o.GetComponent<GridSpaceMono>()?.Space == space)
                .FirstOrDefault();
        }

        private static void ThrowExceptionIfArgumentIsNull(object arg, string paramName, string message) {
			if (arg == null)
				throw new ArgumentNullException(paramName, message);
		}

        private static void ThrowExceptionIfSpaceIsInvalid(GridSpace space, string paramName, string message) {
			if (!space.IsValid())
				throw new InvalidSpaceException(string.Format(message, paramName));
		}
	}
}