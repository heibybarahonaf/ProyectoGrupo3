using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_and_Damage : MonoBehaviour
{
    public int vida = 100;
    public bool invencible = false;
    public float tiempo_invencible = 0.2f;
    public float tiempo_frenado = 0.2f;

    private Animator anim;

    public void Start(){
        anim = GetComponent<Animator>();
    }

        public void RestarVida(int cantidad)
        {
        if(!invencible && vida > 0){
            Debug.Log("Vida restante: " + vida);
            vida -= cantidad;
            anim.Play("Damage");
            StartCoroutine(Invulnerabilidad());
            StartCoroutine(FrenarVelocidad());
            Debug.Log("Vida restante: " + vida);

            if(vida == 0){
                GameOver();
            }
        }
    }

    void GameOver(){
        Debug.Log("GAME OVER!!");
        Time.timeScale = 0;
    }
    
    IEnumerator Invulnerabilidad(){
        invencible = true;
        yield return new WaitForSeconds(tiempo_invencible);
        invencible = false;
    }

    IEnumerator FrenarVelocidad(){
        var velocidadActual = GetComponent<PlayerController>().playerspeed;
        GetComponent<PlayerController>().playerspeed = 0;
        yield return new WaitForSeconds(tiempo_frenado);
        GetComponent<PlayerController>().playerspeed = velocidadActual;
    }
}
