using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces {
	public interface IPathManager {
        List<GridSpace> Find(GridSpace from, GridSpace to);
        void Disconnect(GridSpace space);
        void DisconnectAll(IList<GridSpace> spaces);
        void Reconnect(GridSpace space);
	}
}