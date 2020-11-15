using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using DG.Tweening;

public class WeaponsManager : BehaviourBase
{
    public bool IsSucking { get; private set; }

    [Header("Shoot")]
    public GameObject bulletPrefab;

    public Transform firingPoint;

    public float shootForce = 10;
    public int ammo;
    public float firingTime;
    public float reloadTime;


    private int maxAmmo;
    private float firingTimer;
    private float reloadTimer;
    private bool isReloading;

    [Header("Sucking Settings")]

    public GameObject suckTrigger;
    public Transform suckPoint;
    [SerializeField]
    private bool _canTakeEnemy;
    [SerializeField]
    private bool _isSuckButtonPressed;
    private SuckableObject _objectToSuck;

    public float _suckRadius = 1;

    [Header("Particles")]
    public List<GameObject> particles = new List<GameObject>();

    private void Start()
    {
        maxAmmo = ammo;
        suckTrigger.transform.localScale = Vector3.one;
        _canTakeEnemy = true;
    }

    protected override void CustomUpdate()
    {
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }

        Shoot();

        //Suck
        _isSuckButtonPressed = InputManager.lTrigger;

        if (_isSuckButtonPressed)
            SuckEffectOn();
        else
            SuckEffectOff();

        // TODO : Creare metodo di risucchio

        if (reloadTimer <= 0)
        {
            if (isReloading)
            {
                if (ammo <= 0)
                {
                    ammo = maxAmmo;
                    isReloading = false;
                }
            }
        }
        else
            reloadTimer -= Time.deltaTime;
    }

    protected override void CustomFixedUpdate()
    {
        if (!_canTakeEnemy || IsSucking) return;

        if (_isSuckButtonPressed)
        {
            var colls = Physics.OverlapSphere(suckTrigger.transform.position, _suckRadius);

            if (colls.Length < 0) return;

            _objectToSuck = colls.ToList().Find(c => c.gameObject.GetComponent<SuckableObject>() && c.gameObject.GetComponent<SuckableObject>().IsStunned)?.GetComponent<SuckableObject>();

            if (_objectToSuck == null) return;

            if (_objectToSuck.IsStunned)
            {
                _objectToSuck.IsCatching = true;

                _objectToSuck.transform.position = Vector3.Lerp(_objectToSuck.transform.position, suckPoint.position, Time.deltaTime * 10);
                _objectToSuck.transform.localScale = Vector3.Lerp(_objectToSuck.transform.localScale, Vector3.zero, Time.deltaTime * 3);

                if (_objectToSuck.transform.localScale.x <= 0.1f)
                {
                    _objectToSuck.currentSpawner.RemoveIngredient();
                    Destroy(_objectToSuck.gameObject);
                }
            }
        }
        else
        {
            if (_objectToSuck != null)
            {
                //_objectToSuck.IsCatching = false;
                _objectToSuck = null;
            }
        }
    }

    protected override void PauseChanged(bool pause)
    {
        if (pause)
            SuckEffectOff();
    }

    private void SuckEffectOn()
    {
        foreach (var go in particles)
        {
            if (go == null) continue;

            if(!go.activeInHierarchy)
                go.SetActive(true);
        }
    }

    private void SuckEffectOff()
    {
        foreach (var go in particles)
        {
            if (go == null) continue;

            if (go.activeInHierarchy)
                go.SetActive(false);
        }
    }

    private void Shoot()
    {
        // Attack
        if (InputManager.rTrigger)
        {
            if (ammo > 0)
            {
                if (firingTimer <= 0)
                {
                    float alpha = Vector3.Dot(firingPoint.position, Camera.main.transform.position);
                    GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = firingPoint.TransformDirection(Vector3.forward * shootForce);
                    firingTimer = firingTime;
                    ammo--;
                }
            }
            else
            {
                if (!isReloading)
                    Reload();
            }
        }

    }

    public void Reload()
    {
        reloadTimer = reloadTime;
        isReloading = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(suckTrigger.transform.position,_suckRadius);   
    }
#endif
}
