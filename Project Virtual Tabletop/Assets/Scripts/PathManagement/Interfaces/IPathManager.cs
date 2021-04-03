using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces {
	public interface IPathManager {
        List<GridSpace> Find(GridSpace from, GridSpace to);
	}
}