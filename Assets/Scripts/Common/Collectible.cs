using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Collectible : MonoBehaviour {
	public LayerMask collisionMask;
	BoxCollider2D col;
    
    void Start() {
        col = GetComponent<BoxCollider2D> ();
    }
	
	/*public CollectibleCollisions Raycast() {
		CollectibleCollisions collisions = new CollectibleCollisions();
		collisions.top = RaycastUp();
		collisions.left = RaycastLeft();
		collisions.right = RaycastRight();
		collisions.bottom = RaycastDown();
		
		return collisions;
	}*/
	
	public GameObject SphereCast() {
		Vector2 bottomLeft = new Vector2 (col.bounds.min.x, col.bounds.min.y);
		Vector2 topRight = new Vector2 (col.bounds.max.x, col.bounds.max.y);
		Collider2D hit = Physics2D.OverlapArea(bottomLeft, topRight, collisionMask);
		if (hit) {
			return hit.gameObject;
		}
		return null;
	}
/*
	GameObject RaycastUp() {
		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOriginUp = raycastOrigins.topLeft;
			rayOriginUp += Vector2.right * (verticalRaySpacing * i);
			
			Debug.DrawRay(rayOriginUp, Vector2.up * rayLength,Color.red);
			RaycastHit2D hit = Physics2D.Raycast(rayOriginUp, Vector2.up, rayLength, collisionMask);
			
			if (hit) return hit.collider.gameObject;
		}
		return null;
	}
	
	GameObject RaycastDown() {
		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOriginDown = raycastOrigins.bottomLeft;
			rayOriginDown += Vector2.right * (verticalRaySpacing * i);
			
			Debug.DrawRay(rayOriginDown, Vector2.down * rayLength,Color.red);
			RaycastHit2D hit = Physics2D.Raycast(rayOriginDown, Vector2.down, rayLength, collisionMask);
			
			if (hit) return hit.collider.gameObject;
		}
		return null;
	}
	
	GameObject RaycastLeft() {
		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOriginLeft = raycastOrigins.bottomLeft;
			rayOriginLeft += Vector2.up * (horizontalRaySpacing * i);
			
			Debug.DrawRay(rayOriginLeft, Vector2.left * rayLength,Color.red);
			RaycastHit2D hit = Physics2D.Raycast(rayOriginLeft, Vector2.left, rayLength, collisionMask);
			
			if (hit) return hit.collider.gameObject;
		}
		return null;
	}
	
	GameObject RaycastRight() {
		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOriginRight = raycastOrigins.bottomRight;
			rayOriginRight += Vector2.up * (horizontalRaySpacing * i);
			
			Debug.DrawRay(rayOriginRight, Vector2.right * rayLength,Color.red);
			RaycastHit2D hit = Physics2D.Raycast(rayOriginRight, Vector2.right, rayLength, collisionMask);
			
			if (hit) return hit.collider.gameObject;
		}
		return null;
	}
	
	public struct CollectibleCollisions {
		public GameObject left, right, top, bottom;
	}*/
}
