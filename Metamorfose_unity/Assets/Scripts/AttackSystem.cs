using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class AttackSystem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image imagem;
    [SerializeField] private GameObject ProjectilePrefab;
    private bool canUse = true;
    public Vector3 worldPos;
    private string currentPower;
    public PlayerBehaviour player;
    void Start()
    {
        Cursor.visible = false;
        currentPower = "BasicShoot";
    }

    void Update()
    {
        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        imagem.transform.position = worldPos;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CoolDown());
        }   
    }

    private IEnumerator CoolDown()
    {
        if (canUse)
        {
            canUse = false;
            Shoot();
            yield return new WaitForSeconds(0.5f); // Cooldown duration
            canUse = true;
        }
        
    }
    
    public void Shoot()
    {
        GameObject projectileInstance = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        Projectile proj = projectileInstance.GetComponent<Projectile>();

        Vector2 direction = (worldPos - transform.position).normalized;
        proj.Launch(direction);
    }   
    public void ChangePower(string newPower)
    {
        currentPower = newPower;
    }



}
