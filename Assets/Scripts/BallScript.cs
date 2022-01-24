using System.Collections;
using System.Collections.Generic;
using MilkShake;
using TMPro;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float maxSpeed;//Replace with your max speed 
    public TextMeshProUGUI speedTxt;
    public TrailRenderer trail;
    public ParticleSystem powerHitParticles;
    public ParticleSystem regularHitParticles;
    public ParticleSystem shockWave;
    public ShakePreset powerHitShake;
    public ShakePreset regularHitShake;
    public SoundDef regularHitSound;
    public SoundDef powerHitSound;

    private bool wasHitRecently;
    private float internalMaxSpeed;
    private Renderer selfRenderer;
    private Rigidbody selfRigid;
    private ConstantForce selfConstantForce;
    private Vector3 originalSize;
    private int powerLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        selfRigid = gameObject.GetComponent<Rigidbody>();
        selfRenderer = gameObject.GetComponent<Renderer>();
        selfConstantForce = gameObject.GetComponent<ConstantForce>();
        wasHitRecently = false;
        internalMaxSpeed = maxSpeed;
        originalSize = transform.localScale;
        powerLevel = 1;
        powerHitParticles = Instantiate(powerHitParticles, transform.position, transform.rotation);
        shockWave = Instantiate(shockWave, transform.position, transform.rotation);
        regularHitParticles = Instantiate(regularHitParticles, transform.position, transform.rotation);
    }
    void FixedUpdate()
    {
        if (selfRigid.velocity.magnitude > internalMaxSpeed)
        {
            selfRigid.velocity = selfRigid.velocity.normalized * internalMaxSpeed;
        }
        if (selfRigid.velocity.z < 0 && selfRigid.velocity.magnitude > maxSpeed)
        {
            selfRigid.velocity = selfRigid.velocity.normalized * maxSpeed;
        }
        speedTxt.text = internalMaxSpeed.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            collision.collider.GetComponent<BlockScript>().BreakMe();
            //StartCoroutine(Die(collision.collider.gameObject));
        }
        if (!collision.collider.CompareTag("Bat") && !collision.collider.CompareTag("Ground") && !wasHitRecently)
        {
            //internalMaxSpeed = maxSpeed;
            //ResetPowerUp();
        }

    }

    public void HitByBat(float force, bool powerUp)
    {
        internalMaxSpeed = force;
        //if (powerLevel == 4)
        //{
        //    ResetPowerUp();
        //    Instantiate(gameObject, transform.position, transform.rotation);
        //    return;
        //}
        if (powerUp)
        {
            var main = powerHitParticles.main;
            if(powerLevel == 1)
            {
                main.startColor = Color.red;
            }
            if (powerLevel == 2)
            {
                main.startColor = Color.yellow;
            }
            if (powerLevel >= 3)
            {
                main.startColor = Color.cyan;
            }
            powerHitParticles.transform.position = transform.position;
            powerHitParticles.Play();
            shockWave.transform.position = transform.position;
            shockWave.Play();

            Shaker.ShakeAll(powerHitShake);
            SoundPlayer.PlaySound(powerHitSound);

            //Instantiate(powerHitParticles, transform.position, transform.rotation);
            //Instantiate(shockWave, transform.position, transform.rotation);
            StartCoroutine(FrameFreeze(0.1f));
            Debug.Log("POWER");
            PowerUp();
        } else
        {
            regularHitParticles.transform.position = transform.position;
            regularHitParticles.Play();
            Shaker.ShakeAll(regularHitShake);
            SoundPlayer.PlaySound(regularHitSound);
            //Instantiate(regularHitParticles, transform.position, transform.rotation);
            Debug.Log("RESET");
            ResetPowerUp();
        }
        Handheld.Vibrate();
        StartCoroutine(SetHit());
        //yield return new WaitForSeconds(1);
        //selfRenderer.material.color = Color.gray;
        //internalMaxSpeed = maxSpeed;
    }

    public static IEnumerator FrameFreeze(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }

    private void PowerUp()
    {
        powerLevel++;
        if(powerLevel == 2)
        {
            selfRenderer.material.color = Color.red;
            internalMaxSpeed += 5;
            transform.localScale = originalSize * 1.15f;
            selfRigid.mass = 2f;
            selfConstantForce.force = new Vector3(0, 0, -25 * 1.5f);
            return;
        }
        if(powerLevel == 3)
        {
            selfRenderer.material.color = Color.yellow;
            internalMaxSpeed += 5;
            transform.localScale = originalSize * 1.25f;
            selfRigid.mass = 5;
            selfConstantForce.force = new Vector3(0, 0, -25 * 5);
            return;
        }
        if (powerLevel >= 4)
        {
            selfRenderer.material.color = Color.cyan;
            internalMaxSpeed += 5;
            transform.localScale = originalSize * 1.25f;
            selfRigid.mass = 10;
            selfConstantForce.force = new Vector3(0, 0, -25 * 8);
            return;
        }
    }

    private void ResetPowerUp()
    {
        selfRenderer.material.color = Color.gray;
        selfRigid.mass = 1.5f;
        selfConstantForce.force = new Vector3(0, 0, -25);
        transform.localScale = originalSize;
        powerLevel = 1;
    }

    private IEnumerator SetHit()
    {
        wasHitRecently = true;
        yield return new WaitForSeconds(0.2f);
        wasHitRecently = false;
    }

    public bool WasHitRecently()
    {
        return wasHitRecently;
    }

}
