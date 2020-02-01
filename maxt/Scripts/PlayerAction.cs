using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public enum WeaponSlot { MAGICSPELL, FIREBALL, WATERSTREAM }
    [System.Serializable]
    public struct Weapon
    {
        public WeaponSlot weapon;
        public bool ifUnlocked;
        public Weapon(WeaponSlot weapon, bool ifUnlocked) { this.weapon = weapon; this.ifUnlocked = ifUnlocked; }
    }

    //curr weapon and weapon list
    public int currWeapon = 0;
    public Weapon[] weaponList;

    [HideInInspector]
    public bool ableToPickUp = false; //if we can pick up anything

    public Transform firePoint; //the point where spells initiate
    public Transform PickUpEffectLoc;
    public float pickupEffectLastTime = 1f;

    public Transform[] spellPrefabs; //spell object to shoot

    public float coolDown = 1f;
    private float _attackCoolDown; //private cooldown to control when to fire

    private int size = 3; //szie for weaponlist

    // Start is called before the first frame update
    void Start()
    {
        //set up weapon list
        weaponList[0] = new Weapon(WeaponSlot.MAGICSPELL, true);
        weaponList[1] = new Weapon(WeaponSlot.FIREBALL, false);
        weaponList[2] = new Weapon(WeaponSlot.WATERSTREAM, false);
        size = 3;

        if(PickUpEffectLoc == null)
        {
            Debug.LogError("PlayerAction: no pick up effect location found");
        }

        _attackCoolDown = coolDown;
        if (firePoint == null)
        {
            Debug.LogError("PlayerAction: no firepoint found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //deal with mouse scroll to switch spells
        float mouseScrollAmt = Input.mouseScrollDelta.y;
        if (mouseScrollAmt != 0)
        {
            if (mouseScrollAmt > 0) //switch to next weapon
            {
                currWeapon++;
                if (currWeapon >= size) //set currweapon back to 0 if it's out of bound
                {
                    currWeapon = 0;
                }

                while (!weaponList[currWeapon].ifUnlocked) //scroll until a unlocked weapon is reached
                {
                    currWeapon++;
                    if (currWeapon >= size) //set currweapon back to 0 if it's out of bound
                    {
                        currWeapon = 0;
                    }
                }
            }
            else //switch to previous weapon
            {
                currWeapon--;
                if (currWeapon <= 0) //set currweapon back to 0 if it's out of bound
                {
                    currWeapon = size-1;
                }

                while (!weaponList[currWeapon].ifUnlocked) //scroll until a unlocked weapon is reached
                {
                    currWeapon--;
                    if (currWeapon <= 0) //set currweapon back to 0 if it's out of bound
                    {
                        currWeapon = size-1;
                    }
                }
            }
        }

        //deal with attacking
        _attackCoolDown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && _attackCoolDown <= 0)
        {
            Debug.Log("PlayerAction: Fire!");
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);
            Attack(firePointPos, (mousePos - firePointPos).normalized);
            //after firing, set cool down time back to default;
            _attackCoolDown = coolDown;
        }

        //deal with picking up
        if(ableToPickUp == true && Input.GetKeyDown(KeyCode.E))
        {
            GameObject spellToPick = GameObject.FindGameObjectWithTag("Spell");
            PickUpSpell(spellToPick);

            spellToPick.transform.position = PickUpEffectLoc.transform.position;
            spellToPick.transform.rotation = PickUpEffectLoc.transform.rotation;
            spellToPick.GetComponent<Animator>().Play("PickUpEffectFadingOut");

            Destroy(spellToPick, pickupEffectLastTime);
            ableToPickUp = false;
        }
    }

    //origin: the point where attacks start. Direction
    void Attack(Vector2 origin, Vector2 direction)
    {
        Debug.DrawLine(origin, direction * 100);

        Debug.Log(weaponList[currWeapon].weapon.ToString());

        Transform spellClone = Instantiate(spellPrefabs[currWeapon], firePoint.position, firePoint.rotation);
        spellClone.GetComponent<MoveSpell>().direction = direction;
    }

    void PickUpSpell(GameObject spellToPick)
    {
        switch (spellToPick.name)
        {
            case "FireBallSpell":
                weaponList[1].ifUnlocked = true;
                break;
            case "WaterStream":
                weaponList[2].ifUnlocked = true;
                break;
        }
    }
}
