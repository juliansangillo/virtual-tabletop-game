using UnityEngine;
using Zenject;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Components.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Components {
	public class MoveToken : MonoBehaviour {
        [SerializeField] private GridSpaceMono currentSpace;
        [SerializeField] private int maxHeight;
        [SerializeField] private float lerpDuration;

        private Token token;
        private Vector3 initialPosition;

        private IGridManager gridManager;
        private ILerpable lerpable;
        private IRaycastable raycastable;
        private SignalBus signalBus;

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

        public ISelectEffect InitialSelectEffect { get; private set; }
        public ISelectEffect CurrentSelectEffect { get; internal set; }

        [Inject]
        public void Construct(IGridManager gridManager, ILerpable lerpable, IRaycastable raycastable, SignalBus signalBus) {
            this.gridManager = gridManager;
            this.lerpable = lerpable;
            this.raycastable = raycastable;
            this.signalBus = signalBus;
        }

        public void Start() {
            token = (Token)gridManager.GetElementOn(currentSpace.Space);
            initialPosition = transform.position;

            InitialSelectEffect = currentSpace.SelectEffect;
            CurrentSelectEffect = currentSpace.SelectEffect;
        }

        public void OnMouseDown() {
            Vector3 source = transform.position;
            Vector3 destination = new Vector3(source.x, maxHeight, source.z);
            StopAllCoroutines();
            StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => transform.position = current));

            CurrentSelectEffect.Play();

            signalBus.Fire(new TokenSelectedSignal(token.CurrentSpace));
        }

        public void OnMouseDrag() {
            GameObject hitObject = raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG);
            if(hitObject != null)
                MoveToNewTileIfSpaceIsValid(hitObject);
        }

        public void OnMouseUp() {
            CurrentSelectEffect.Stop();

            GameObject hitObject = raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG);
            if(hitObject != null)
                PlaceDownOnNewTileIfSpaceIsEmpty(hitObject);
            else {
                SnapBackToInitialPosition();
                SnapBackToInitialSelectEffect();
                signalBus.Fire(new TokenReleasedSignal(token.CurrentSpace, new GridSpace(-1, -1)));
            }
        }

        private void MoveToNewTileIfSpaceIsValid(GameObject hitObject) {
            GridSpace newSpace = hitObject.GetComponent<GridSpaceMono>().Space;
            Vector3 destination = new Vector3(hitObject.transform.position.x, transform.position.y, hitObject.transform.position.z);

            if(IsNotCurrentPosition(destination) && IsSpaceEmpty(newSpace)) {
                transform.position = destination;
                UpdateSelectEffects(hitObject);
                signalBus.Fire(new TokenDraggedSignal(token.CurrentSpace, newSpace));
            }
        }

        private void UpdateSelectEffects(GameObject hitObject) {
            ISelectEffect newSelectEffect = hitObject.GetComponent<GridSpaceMono>().SelectEffect;

            CurrentSelectEffect.Stop();
            newSelectEffect.Play();
            CurrentSelectEffect = newSelectEffect;
        }

        private bool IsNotCurrentPosition(Vector3 destination) {
            return Vector3.Distance(destination, transform.position) != 0f;
        }

        private void PlaceDownOnNewTileIfSpaceIsEmpty(GameObject hitObject) {
            GridSpace oldSpace = token.CurrentSpace;
            GridSpace newSpace = hitObject.GetComponent<GridSpaceMono>().Space;
            if(IsSpaceEmpty(newSpace)) {
                MoveTokenOnGrid(newSpace);
                PlaceDownOnTile();
                UpdateInitialSelectEffect();
                signalBus.Fire(new TokenReleasedSignal(oldSpace, newSpace));
            }
            else {
                SnapBackToInitialPosition();
                SnapBackToInitialSelectEffect();
                signalBus.Fire(new TokenReleasedSignal(oldSpace, new GridSpace(-1, -1)));
            }
        }

        private void MoveTokenOnGrid(GridSpace newSpace) {
            gridManager.Move(token.CurrentSpace, newSpace);
            token.CurrentSpace = newSpace;
        }

        private void PlaceDownOnTile() {
            Vector3 source = transform.position;
            Vector3 destination = new Vector3(source.x, 0, source.z);
            StopAllCoroutines();
            StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => {
                transform.position = current;
                initialPosition = transform.position;
            }));
        }

        private void UpdateInitialSelectEffect() {
            InitialSelectEffect = CurrentSelectEffect;
        }

        private void SnapBackToInitialPosition() {
            transform.position = initialPosition;
        }

        private void SnapBackToInitialSelectEffect() {
            CurrentSelectEffect = InitialSelectEffect;
        }

        private bool IsSpaceEmpty(GridSpace space) {
            return gridManager.IsEmpty(space);
        }
    }
}