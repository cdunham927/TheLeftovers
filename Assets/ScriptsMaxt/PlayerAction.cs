using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    public enum WeaponSlot { MAGICSPELL, FIREBALL, LIGHTING, ICE, WATER }
    [System.Serializable]
    public struct Weapon
    {
        public WeaponSlot weapon;
        public bool ifUnlocked;
        public float coolDown;
        public Weapon(WeaponSlot weapon, bool ifUnlocked, float coolDown) { this.weapon = weapon; this.ifUnlocked = ifUnlocked; this.coolDown = coolDown; }
    }

    //curr weapon and weapon list
    public Weapon[] weaponList;
    public int currWeapon = 0;
    public List<int> unlockedWeapons;
    private int indexUnlokedWeapon = 0;

    [HideInInspector]
    public bool ableToPickUp = false; //if we can pick up anything

    public Transform firePoint; //the point where spells initiate
    public Transform PickUpEffectLoc;
    public float pickupEffectLastTime = 1f;

    public Transform[] spellPrefabs; //spell object to shoot

    public float coolDown = 1f;
    private float lastTime = 0f; //last time when we shoot a spell

    public float prefabRotateOffSet = 180f;
    public float spellSpawnYOffSet = 4f;
    public float spellLastTime = 1f;

    public GameObject book;
    public Material[] materials;

    public Image cooldownImage;
    public float lerpSpd = 7.5f;

    //AUDIO
    AudioSource src;
    public AudioClip pickup;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        //set up weapon list
        weaponList[0] = new Weapon(WeaponSlot.MAGICSPELL, true, 1);
        weaponList[1] = new Weapon(WeaponSlot.FIREBALL, false, 2);
        weaponList[2] = new Weapon(WeaponSlot.LIGHTING, false, 5);
        weaponList[3] = new Weapon(WeaponSlot.ICE, false, 2);
        weaponList[4] = new Weapon(WeaponSlot.WATER, false, 2);

        unlockedWeapons.Add(0); //default magic projectile
        indexUnlokedWeapon = 0;

        if (PickUpEffectLoc == null)
        {
            Debug.LogError("PlayerAction: no pick up effect location found");
        }

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
                indexUnlokedWeapon++;
                if(indexUnlokedWeapon >= unlockedWeapons.Count)
                {
                    indexUnlokedWeapon = 0;
                }
            } else //switch to previous weapon
            {
                indexUnlokedWeapon--;
                if (indexUnlokedWeapon < 0)
                {
                    indexUnlokedWeapon = unlockedWeapons.Count-1;
                }
            }
            //Debug.Log(indexUnlokedWeapon);
            currWeapon = unlockedWeapons[indexUnlokedWeapon];
        }

        //deal with attacking
        if (Input.GetButtonDown("Fire1") && (Time.time - lastTime) >= weaponList[currWeapon].coolDown)
        {
            //Debug.Log("PlayerAction: Fire!");
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 firePointPos = new Vector2(firePoint.position.x, firePoint.position.y);

            //angel the spell should face
            float angel = Mathf.Atan2(mousePos.y - firePoint.position.y, mousePos.x - firePoint.position.x) * (180 / Mathf.PI);

            Attack(firePointPos, (mousePos - firePointPos), angel);
            //after firing, set cool down time back to default;
            lastTime = Time.time;
            //cooldownImage.fillAmount - 0f;
        }

        cooldownImage.fillAmount = Mathf.Lerp(cooldownImage.fillAmount, ((Time.time - lastTime) / weaponList[currWeapon].coolDown), lerpSpd * Time.deltaTime);

        //deal with picking up
        if (ableToPickUp == true && Input.GetKeyDown(KeyCode.E))
        {
            GameObject spellToPick = GameObject.FindGameObjectWithTag("Spell");
            PickUpSpell(spellToPick);
            //disable unused components in spell scroll
            spellToPick.GetComponent<CheckRange>().enabled = false;
            spellToPick.transform.GetChild(0).gameObject.SetActive(false);

            spellToPick.transform.position = PickUpEffectLoc.transform.position;
            spellToPick.GetComponent<Animator>().Play("PickUpEffectFadingOut");

            Destroy(spellToPick, pickupEffectLastTime);
            src.PlayOneShot(pickup);
            ableToPickUp = false;
        }
    }

    //origin: the point where attacks start. Direction
    void Attack(Vector2 origin, Vector2 direction, float angel)
    {
        Debug.DrawLine(origin, direction * 100);
        //Debug.Log(weaponList[currWeapon].weapon.ToString());

        Vector3 mousePos = direction + origin;
        direction = direction.normalized;

        if (currWeapon == 0) //default magic projectile
        {
            prefabRotateOffSet = 180;
            Transform spellClone = Instantiate(spellPrefabs[currWeapon], firePoint.position, Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, firePoint.rotation.z + angel + prefabRotateOffSet));
            spellClone.GetComponent<MoveSpell>().direction = direction;
            spellClone.GetComponent<Animator>().SetBool("Flying",true);
        } else if(currWeapon == 1) //fireball
        {
            Transform spellClone = Instantiate(spellPrefabs[currWeapon], firePoint.position, firePoint.rotation);
            spellClone.GetComponent<MoveSpell>().direction = direction;
            spellClone.GetComponent<Animator>().SetBool("FireRotating", true);
        } else if(currWeapon == 2)//lightning
        {
            Transform spellClone = Instantiate(spellPrefabs[currWeapon], new Vector3(mousePos.x, mousePos.y + spellSpawnYOffSet, 0), firePoint.rotation);
            spellClone.GetChild(1).GetComponent<Animator>().Play("Lightning");
            Destroy(spellClone.gameObject, spellLastTime);
        } else if(currWeapon == 3) //freeze
        {
            prefabRotateOffSet = 0;
            Transform spellClone = Instantiate(spellPrefabs[currWeapon], firePoint.position, Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, firePoint.rotation.z + angel + prefabRotateOffSet));
            spellClone.GetComponent<MoveSpell>().direction = direction;
            spellClone.GetComponent<Animator>().Play("Freeze");
        } else //water
        {
            prefabRotateOffSet = 180;
            Transform spellClone = Instantiate(spellPrefabs[currWeapon], firePoint.position, Quaternion.Euler(firePoint.rotation.x, firePoint.rotation.y, firePoint.rotation.z + angel + prefabRotateOffSet));
            spellClone.GetComponent<MoveSpell>().direction = direction;
            spellClone.GetComponent<Animator>().SetBool("WaterFloating", true);
        }
    }

    public void StopBook()
    {
        book.SetActive(false);
    }

    void PickUpSpell(GameObject spellToPick)
    {
        switch (spellToPick.name)
        {
            case "FireBallScroll(Clone)":
                book.SetActive(true);
                book.GetComponentInChildren<Renderer>().material = materials[0];
                Invoke("StopBook", 1f);
                weaponList[1].ifUnlocked = true;
                unlockedWeapons.Add(1);
                unlockedWeapons.Sort();
                break;
            case "LightningScroll(Clone)":
                book.SetActive(true);
                book.GetComponentInChildren<Renderer>().material = materials[1];
                Invoke("StopBook", 1f);
                weaponList[2].ifUnlocked = true;
                unlockedWeapons.Add(2);
                unlockedWeapons.Sort();
                break;
            case "IceScroll(Clone)":
                book.SetActive(true);
                book.GetComponentInChildren<Renderer>().material = materials[2];
                Invoke("StopBook", 1f);
                weaponList[3].ifUnlocked = true;
                unlockedWeapons.Add(3);
                unlockedWeapons.Sort();
                break;
            case "WaterScroll(Clone)":
                book.SetActive(true);
                book.GetComponentInChildren<Renderer>().material = materials[3];
                Invoke("StopBook", 1f);
                weaponList[4].ifUnlocked = true;
                unlockedWeapons.Add(4);
                unlockedWeapons.Sort();
                break;
        }
    }
}
