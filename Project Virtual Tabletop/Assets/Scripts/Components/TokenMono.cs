using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.GameController.Interfaces;
using UnityEngine;
using Zenject;
using Space = ProjectVirtualTabletop.Entities.Space;

public class TokenMono : MonoBehaviour {
	[SerializeField] private SpaceMono spaceMono;
    [SerializeField] private int deltaY;

    private Token token;
    private Vector3 initialPosition;

    private IGridManager gridManager;

    

    public SpaceMono SpaceMono {
        set {
            this.spaceMono = value;
        }
    }

    public int DeltaY {
        set {
            this.deltaY = value;
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
        token = (Token)gridManager.GetElementOn(spaceMono.Space);
        initialPosition = transform.position;
    }

    public void OnMouseDown() {
        transform.Translate(Vector3.up * deltaY);
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
            Space newSpace = hit.collider.GetComponent<SpaceMono>().Space;

            if(gridManager.IsEmpty(newSpace)) {
                gridManager.Move(token.CurrentSpace, newSpace);
                token.CurrentSpace = newSpace;
                transform.Translate(Vector3.down * deltaY);
                initialPosition = transform.position;
            }
            else
                transform.position = initialPosition;
        }
        else
            transform.position = initialPosition;
    }
}
