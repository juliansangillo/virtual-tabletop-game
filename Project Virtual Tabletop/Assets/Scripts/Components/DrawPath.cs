using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using UnityEngine;
using Zenject;

public class DrawPath : MonoBehaviour {
	private IPathManager pathManager;

    [Inject]
    public void Construct(IPathManager pathManager) {
        this.pathManager = pathManager;
    }

	public void Start() {

	}
}
