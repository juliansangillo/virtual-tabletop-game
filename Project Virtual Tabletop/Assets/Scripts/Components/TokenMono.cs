using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.GameController.Interfaces;
using UnityEngine;
using Zenject;

public class TokenMono : MonoBehaviour {
	[SerializeField] private GridSpaceMono gridSpaceMono;
    [SerializeField] private int maxHeight;
    [SerializeField] [Range(0, 1)] private float lerpDelta;

    private Token token;
    private Vector3 initialPosition;

    private IGridManager gridManager;

    public GridSpaceMono GridSpaceMono {
        set {
            this.gridSpaceMono = value;
        }
    }

    public int MaxHeight {
        set {
            this.maxHeight = value;
        }
    }

    public float LerpDelta {
        set {
            this.lerpDelta = value;
        }
    }

    public Token Token {
        get {
            return this.token;
        }
    }

    [Inject]
    public void Construct(IGridManager gridManager) {
        this.gridManager = gridManager;
    }

    public void Start() {
        token = (Token)gridManager.GetElementOn(gridSpaceMono.Space);
        initialPosition = transform.position;
    }

    public void OnMouseDown() {
        StartCoroutine(TranslateUp());
    }

    private IEnumerator TranslateUp() {
        float lerpPosition = 0f;
        do {
            lerpPosition += lerpDelta;
            if(lerpPosition >= 1f)
                lerpPosition = 1f;

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, maxHeight, transform.position.z), lerpPosition);

            yield return null;
        } while(lerpPosition < 1f);
    }

    public void OnMouseDrag() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = Physics.RaycastAll(ray).FirstOrDefault(h => h.collider.tag == "Tile");

        if(hit.collider != null) {
            Transform other = hit.collider.transform;
            transform.position = new Vector3(other.position.x, transform.position.y, other.position.z);
        }
    }

    public void OnMouseUp() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = Physics.RaycastAll(ray).FirstOrDefault(h => h.collider.tag == "Tile");

        if(hit.collider != null) {
            GridSpace newSpace = hit.collider.GetComponent<GridSpaceMono>().Space;

            if(gridManager.IsEmpty(newSpace)) {
                gridManager.Move(token.CurrentSpace, newSpace);
                token.CurrentSpace = newSpace;
                StartCoroutine(TranslateDown());
            }
            else
                transform.position = initialPosition;
        }
        else
            transform.position = initialPosition;
    }

    private IEnumerator TranslateDown() {
        float lerpPosition = 0f;
        do {
            lerpPosition += lerpDelta;
            if(lerpPosition >= 1f)
                lerpPosition = 1f;

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), lerpPosition);

            yield return null;
        } while(lerpPosition < 1f);

        initialPosition = transform.position;
    }
}
