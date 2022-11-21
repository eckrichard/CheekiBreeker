using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public float health;            //current health of the player
    public float maxHealth;         //maximum health of the player
    private MovementHandler movementHandler;
    public AudioClip[] damageYells;
    public AudioClip[] shootingYells;
    public AudioSource audioSource;

    public GameObject WeaponsHandler;
    public Image HpBar;
    public Image RELOADING_WARN;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI AmmoText;

    /**
     * Start function of handler
     * Loads area, where the player saved
     * sets the player's health to full
     */
    void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
        audioSource.loop = false;
        health = maxHealth;
    }

    public void Update()
    {
        Transform firstActiveWeapon = WeaponsHandler.transform.GetChild(1); 

        for (int i = 0; i < WeaponsHandler.transform.childCount; i++)
        {
            if (WeaponsHandler.transform.GetChild(i).gameObject.activeSelf == true)
            {
                firstActiveWeapon = WeaponsHandler.transform.GetChild(i);
            }
        }

        firstActiveWeapon.GetComponent<WeaponScript>().onShooting -= yellShooting;

        firstActiveWeapon.GetComponent<WeaponScript>().onShooting += yellShooting;

        if (firstActiveWeapon.GetComponent<WeaponScript>().isRealoading())
        {
            RELOADING_WARN.enabled = true;
        } else
        {
            RELOADING_WARN.enabled = false;
        }

        AmmoText.text = (firstActiveWeapon.GetComponent<WeaponScript>().isRealoading() ? 0 : firstActiveWeapon.GetComponent<WeaponScript>().CurrentMagazine + 1)  
                         + " / " + firstActiveWeapon.GetComponent<WeaponScript>().startMag;
        HpText.text = health + " / " + maxHealth;
    }

    /**
     * reduces the player's health
     * @param float damage
     */
    public void TakeDamage(float damage, Transform collider = null)
    {

        health -= damage;
        HpBar.fillAmount = health / maxHealth;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(damageYells[Random.Range(0, damageYells.Length)]);
        }
            

        if (collider != null)
        {
            StartCoroutine(movementHandler.Knockback(collider));
                    
        }
        if(health <= 0)
        {
            //TODO: Death
        }
        
    }

    public void yellShooting()
    {
        if (!audioSource.isPlaying && Random.Range(0,7) >=6 )
        {
            audioSource.PlayOneShot(shootingYells[Random.Range(0, damageYells.Length)]);
        }
    }


    
}
