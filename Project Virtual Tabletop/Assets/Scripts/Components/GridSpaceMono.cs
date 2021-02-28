using ProjectVirtualTabletop.Entities;
using UnityEngine;

public class GridSpaceMono : MonoBehaviour {
	[SerializeField] private int row;
    [SerializeField] private int col;

    private GridSpace space;

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
    
    public GridSpace Space {
        get {
            return this.space;
        }
    }
    
    public void Awake() {
        space = new GridSpace(row, col);
	}
}
