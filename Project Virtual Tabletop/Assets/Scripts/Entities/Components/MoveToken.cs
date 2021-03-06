using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components {
	public class MoveToken : MonoBehaviour {
        [SerializeField] private GridSpaceMono currentSpace;
        [SerializeField] private int maxHeight;
        [SerializeField] private float lerpDuration;

        private Token token;
        private Vector3 initialPosition;

        private IGridManager gridManager;
        private ILerpable lerpable;
        private IRaycastable raycastable;

        public GridSpaceMono CurrentSpace {
            set {
                this.currentSpace = value;
            }
        }

        public int MaxHeight {
            set {
                this.maxHeight = value;
            }
        }

        public float LerpDuration {
            set {
                this.lerpDuration = value;
            }
        }
        
        public Token Token {
            get {
                return this.token;
            }
        }

        public Vector3 InitialPosition {
            get {
                return this.initialPosition;
            }
        }

        [Inject]
        public void Construct(IGridManager gridManager, ILerpable lerpable, IRaycastable raycastable) {
            this.gridManager = gridManager;
            this.lerpable = lerpable;
            this.raycastable = raycastable;
        }

        public void Start() {
            token = (Token)gridManager.GetElementOn(currentSpace.Space);
            initialPosition = transform.position;
        }

        public void OnMouseDown() {
            Vector3 source = transform.position;
            Vector3 destination = new Vector3(source.x, maxHeight, source.z);
            StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => transform.position = current));
        }

        public void OnMouseDrag() {
            GameObject hitObject = raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG);
            if(hitObject != null) {
                MoveToNewTileIfSpaceIsValid(hitObject);
            }
        }

        private void MoveToNewTileIfSpaceIsValid(GameObject hitObject) {
            GridSpace newSpace = hitObject.GetComponent<GridSpaceMono>().Space;
            Vector3 source = transform.position;
            Vector3 destination = new Vector3(hitObject.transform.position.x, source.y, hitObject.transform.position.z);

            if(IsSpaceValid(newSpace))
                StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => transform.position = current));
        }

        public void OnMouseUp() {
            GameObject hitObject = raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG);
            if(hitObject != null) {
                PlaceDownOnNewTileIfSpaceIsValid(hitObject);
            }
            else
                SnapBackToInitialPosition();
        }

        private void PlaceDownOnNewTileIfSpaceIsValid(GameObject hitObject) {
            GridSpace newSpace = hitObject.GetComponent<GridSpaceMono>().Space;

            if(IsSpaceValid(newSpace)) {
                MoveTokenOnGrid(newSpace);
                PlaceDownOnTile();
            }
            else
                SnapBackToInitialPosition();
        }

        private void MoveTokenOnGrid(GridSpace newSpace) {
            gridManager.Move(token.CurrentSpace, newSpace);
            token.CurrentSpace = newSpace;
        }

        private void PlaceDownOnTile() {
            Vector3 source = transform.position;
            Vector3 destination = new Vector3(source.x, 0, source.z);
            StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => {
                transform.position = current;
                initialPosition = transform.position;
            }));
        }

        private void SnapBackToInitialPosition() {
            transform.position = initialPosition;
        }

        private bool IsSpaceValid(GridSpace space) {
            return IsSpaceNotTheCurrentSpace(space) && IsSpaceEmpty(space);
        }

        private bool IsSpaceNotTheCurrentSpace(GridSpace space) {
            return space != token.CurrentSpace;
        }

        private bool IsSpaceEmpty(GridSpace space) {
            return gridManager.IsEmpty(space);
        }
    }
}