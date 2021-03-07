using UnityEngine;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components {
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

        public ISelectEffect SelectEffect { get; set; }
        
        public void Awake() {
            space = new GridSpace(row, col);

            SelectEffect = GetComponent<ISelectEffect>();
        }
    }
}