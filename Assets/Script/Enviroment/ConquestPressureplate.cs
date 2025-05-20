using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestPressureplate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer openSprite;
   [SerializeField] private SpriteRenderer closedSprite;



    //when the player steps on the pressure plate, it will trigger first conquest sequence

    //1. pressure pressed -> enemy spawns and plate stays closed -> whene enemey dies, plate opens and first tourch is lit
    //2. pressure pressed -> 2enemy spawns and plate stays closed -> whene enemey dies, plate opens and second tourch is lit
    //3. pressure pressed -> 3enemy spawns and plate stays closed -> whene enemey dies, plate opens and third tourch is lit
    //4. pressure pressed -> 4enemy spawns and plate stays closed -> whene enemey dies, plate opens and fourth tourch is lit
    //5. Chest appears from the ground with loot.
}
