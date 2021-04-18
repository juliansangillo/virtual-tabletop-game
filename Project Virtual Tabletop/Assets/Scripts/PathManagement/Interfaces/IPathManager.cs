using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces {
	public interface IPathManager {
        GridPath Find(GridSpace from, GridSpace to);
        void Disconnect(GridSpace space);
        void DisconnectAll(IList<GridSpace> spaces);
        void Reconnect(GridSpace space);
	}
}