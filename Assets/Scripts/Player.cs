using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string nomePersonagem;
    [SerializeField] private int vida;
    [SerializeField] private int ataque;
    [SerializeField] private int defesa;
    [SerializeField] private int especial;
    [SerializeField] private bool estahVivo = true;
    [SerializeField] private DiretorBatalha dB;
    [SerializeField] private Sprite spriteDerrota;
    [SerializeField] private bool ehHeroi;
    [SerializeField] private new Camera camera;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private FalasPersonagem falasPersonagem;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        //if (falasPersonagem && !ehHeroi)
        //{
        //    falasPersonagem = GetComponent<FalasPersonagem>();
        //}
    }

    public void FalaDoPersonagem(string tipoDeFala)
    {
        if (falasPersonagem != null && !ehHeroi && tipoDeFala == "Ataque")
        {
            falasPersonagem.FalaDeAtaque();
        }
        else if (falasPersonagem != null && !ehHeroi && tipoDeFala == "Defesa")
        {
            falasPersonagem.FalaDeDefesa();
        }
    }

    public string GetNomePersonagem()
    {
        return nomePersonagem;
    }

    public int GetVida()
    {
        return vida;
    }

    public bool VerificaVida() //retorna true se o jogador esta vivo e false se esta morto
    {
        return estahVivo;
    }

    public bool VerificaEspecial() //retorna true se o jogador tem especial e false se nao tem
    {
        if (especial >= 3)
        {
            dB.RecebeTexto($"{nomePersonagem} especial pronto!");
            return true;
        }
        else
        {
            return false;
        }
    }

    public int ValorEspecial()
    {
        return especial;
    }

    public int Ataque()
    {
        int valorAtaque = Random.Range(0, ataque);

        especial++;

        AnimaAtaque();

        if (valorAtaque > 0)
        {
            FalaDoPersonagem("Ataque");
            dB.RecebeTexto("Corte sagrado!");
            dB.RecebeTexto($"{nomePersonagem} ataca com {valorAtaque}");
        }
        else
        {
            dB.RecebeTexto($"{nomePersonagem} erra o ataque.");
        }


        return valorAtaque;
    }

    public int Defesa()
    {
        int valorDefesa = Random.Range(0, defesa);

        if (valorDefesa > 0)
        {
            FalaDoPersonagem("Defesa");
            dB.RecebeTexto($"{nomePersonagem} defende com {valorDefesa}");
        }
        else
        {
            dB.RecebeTexto($"{nomePersonagem} nao consegue defender.");
        }


        return valorDefesa;
    }

    public int Especial()
    {
        int valorEspecial = Random.Range(20, ataque);
        int chanceDeDobrar = Random.Range(0, 100);
        int fatorMultiplicador = especial;

        AnimaAtaque();

        if (chanceDeDobrar >= 90 && especial >= 3)
        {
            int valorEspecialDobrado = (valorEspecial * 2) + fatorMultiplicador;
            FalaDoPersonagem("Ataque");
            dB.RecebeTexto("Sede de Vinguança!");
            dB.RecebeTexto($"{nomePersonagem} ataca com {valorEspecialDobrado}");
            especial = 0;
            return valorEspecialDobrado;
        }
        else if (chanceDeDobrar < 90 && especial >= 3)
        {
            FalaDoPersonagem("Ataque");
            dB.RecebeTexto("Furia das laminas sagradas!");
            dB.RecebeTexto($"{nomePersonagem} ataca com {valorEspecial}");
            especial = 0;
            return valorEspecial;
        }
        else
        {
            dB.RecebeTexto("Seu especial nao esta carregado!");
            return 0;
        }
    }

    public void LevarDano(int dano)
    {
        int danoFinal = dano - Defesa();

        if (danoFinal <= 0)
        {
            StartCoroutine(TocarDefesa());
        }
        else if (danoFinal <= 25)
        {
            StartCoroutine(TocarDanoNormal(danoFinal));
        }
        else
        {
            StartCoroutine(TocarDanoMaximo(danoFinal));
        }

        if (estahVivo)
        {
            Debug.Log("");
            Debug.Log($"{nomePersonagem}, vida: {vida}");
        }
        else
        {
            dB.RecebeTexto($"{nomePersonagem}, morreu!");
        }

    }
    private void DefineVida() //Verifica o valor da vida e define como morto
    {
        if (vida <= 0)
        {
            spriteRenderer.sprite = spriteDerrota;
            vida = 0;
            estahVivo = false; //Ta morto
        }
    }

    private void AnimaAtaque()
    {
        if (ehHeroi)
        {
            anim.SetTrigger("playerAtaque");
        }
        else
        {
            anim.SetTrigger("Ataque");
        }
    }

    IEnumerator TocarDefesa()
    {
        dB.RecebeTexto($"{nomePersonagem} consegue se defender!");
        anim.SetTrigger("Defesa");
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator TocarDanoNormal(int danoFinal)
    {
        dB.RecebeTexto($"{nomePersonagem} leva dano de {danoFinal}.");
        anim.SetTrigger("Dano");
        yield return new WaitForSeconds(0.5f);
        vida -= danoFinal; //vida = vida - danoFinal;
        DefineVida();
    }

    IEnumerator TocarDanoMaximo(int danoFinal)
    {
        dB.RecebeTexto($"{nomePersonagem} toma uma porrada de {danoFinal}.");
        anim.SetTrigger("Dano");
        yield return new WaitForSeconds(0.5f);
        CameraTreme(danoFinal * 0.1f);
        vida -= danoFinal;
        DefineVida();
    }

    private void CameraTreme(float magnitude)
    {
        camera.GetComponent<CameraShake>().ShakeCamera(0.5f, magnitude);
    }
}

internal class FalasPersonagem
{
    internal void FalaDeAtaque()
    {
        throw new System.NotImplementedException();
    }

    internal void FalaDeDefesa()
    {
        throw new System.NotImplementedException();
    }
}

internal class CameraShake
{
    internal void ShakeCamera(float v, float magnitude)
    {
        throw new System.NotImplementedException();
    }
}