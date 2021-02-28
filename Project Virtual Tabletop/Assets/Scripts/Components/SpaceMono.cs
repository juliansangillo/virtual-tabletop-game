using UnityEngine;
using Space = ProjectVirtualTabletop.Entities.Space;

public class SpaceMono : MonoBehaviour {
	[SerializeField] private int row;
    [SerializeField] private int col;

    private Space space;

    public int Row {
        set {
            this.row = value;
        }
    }
    
    public int Col {
        set {
            this.col = value;
        }
    }
    
    public Space Space {
        get {
            return this.space;
        }
    }
    
    public void Awake() {
        space = new Space(row, col);
	}
}
