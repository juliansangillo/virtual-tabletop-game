using System.Collections;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;
using UnityEngine;
using Zenject;

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
            GameObject hitObject = raycastable.CastRayForTag("Tile");
            if(hitObject != null) {
                Vector3 source = transform.position;
                Vector3 destination = new Vector3(hitObject.transform.position.x, source.y, hitObject.transform.position.z);

                if(Vector3.Distance(source, destination) != 0f)
                    StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => transform.position = current));
            }
        }

        public void OnMouseUp() {
            GameObject hitObject = raycastable.CastRayForTag("Tile");
            if(hitObject != null) {
                GridSpace newSpace = hitObject.GetComponent<GridSpaceMono>().Space;

                if(gridManager.IsEmpty(newSpace)) {
                    gridManager.Move(token.CurrentSpace, newSpace);
                    token.CurrentSpace = newSpace;

                    Vector3 source = transform.position;
                    Vector3 destination = new Vector3(source.x, 0, source.z);
                    StartCoroutine(lerpable.Lerp(source, destination, lerpDuration, current => {
                        transform.position = current;
                        initialPosition = transform.position;
                    }));
                }
                else
                    transform.position = initialPosition;
            }
            else
                transform.position = initialPosition;
        }
    }
}