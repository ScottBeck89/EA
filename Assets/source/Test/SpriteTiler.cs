using UnityEngine;
using System.Collections;

// @NOTE the attached sprite's position should be "top left" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
[RequireComponent( typeof( SpriteRenderer ) )]

// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
public class SpriteTiler : MonoBehaviour
{
    SpriteRenderer sprite;
     void Awake () {
 
         sprite = GetComponent<SpriteRenderer>();
         if(!sprite.sprite.pivot.Equals(SpriteAlignment.TopRight)){
             Debug.LogError("You forgot change the sprite pivot to Top Right." + sprite.sprite.pivot);
         }
         Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);
 
         GameObject childPrefab = new GameObject();
 
         SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
         childPrefab.transform.position = transform.position;
         childSprite.sprite = sprite.sprite;
 
         GameObject child;
         for (int i = 0, h = (int)Mathf.Round(sprite.bounds.size.y); i*spriteSize.y < h; i++) {
             for (int j = 0, w = (int)Mathf.Round(sprite.bounds.size.x); j*spriteSize.x < w; j++) {
                 child = Instantiate(childPrefab) as GameObject;
                 child.transform.position = transform.position - (new Vector3(spriteSize.x*j, spriteSize.y*i, 0));
                 child.transform.parent = transform;
             }
         }
 
         Destroy(childPrefab);
         sprite.enabled = false;
 
     }
}