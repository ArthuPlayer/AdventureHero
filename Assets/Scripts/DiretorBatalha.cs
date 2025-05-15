using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiretorBatalha : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Player inimigo;
    [SerializeField] TextMeshProUGUI vidaPlayer;
    [SerializeField] TextMeshProUGUI vidaInimigo;
    [SerializeField] TextMeshProUGUI nomePlayer;
    [SerializeField] TextMeshProUGUI nomeInimigo;
    [SerializeField] TextMeshProUGUI informativo;
    [SerializeField] TextMeshProUGUI indicadorEspecial;
    [SerializeField] GameObject textoTextoVitoria;
    [SerializeField] GameObject textoTextoDerrota;
    [SerializeField] Button botaoEspecial;
    [SerializeField] Button botaoAtaque;
    string turno = "Player";
    bool verificadorDeTurno = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaPlayer.text = player.GetVida().ToString();
        vidaInimigo.text = inimigo.GetVida().ToString();
        nomePlayer.text = player.GetNomePersonagem();
        nomeInimigo.text = inimigo.GetNomePersonagem();
        indicadorEspecial.text = player.ValorEspecial().ToString();
        botaoEspecial.interactable = false;
    }

    void Update()
    {
        AtualizaDadosTela();

        if (turno == "Player" && verificadorDeTurno && player.VerificaVida())
        {
            botaoAtaque.interactable = true;

            if (player.VerificaEspecial())
            {
                botaoEspecial.interactable = true;
            }
            else
            {
                botaoEspecial.interactable = false;
            }

            verificadorDeTurno = false;
        }
        else if (turno == "Inimigo" && verificadorDeTurno && inimigo.VerificaVida())
        {
            StartCoroutine(AtaqueInimigo());
        }

        VerificaVitoria();
    }

    public void AtaquePlayer()
    {
        inimigo.LevarDano(player.Ataque());
        StartCoroutine(AtaqueP());
    }

    public void AtaqueEspecial()
    {
        inimigo.LevarDano(player.Especial());
        StartCoroutine(AtaqueP());
    }

    private void AtualizaDadosTela()
    {
        vidaPlayer.text = player.GetVida().ToString();
        vidaInimigo.text = inimigo.GetVida().ToString();
    }

    public void RecebeTexto(string texto)
    {
        StartCoroutine(ExibeTexto(texto));
    }

    private IEnumerator ExibeTexto(string texto)
    {
        informativo.text += texto + "\n";
        yield return new WaitForSeconds(5f);
        informativo.text = "";
    }

    private IEnumerator AtaqueInimigo()
    {
        verificadorDeTurno = false;

        if (turno == "Inimigo")
        {
            botaoAtaque.interactable = false;
            botaoEspecial.interactable = false;
            player.LevarDano(inimigo.Ataque());
            yield return new WaitForSeconds(5f);
            verificadorDeTurno = true;
            turno = "Player";
        }
    }

    private IEnumerator AtaqueP()
    {
        verificadorDeTurno = false;
        botaoAtaque.interactable = false;
        botaoEspecial.interactable = false;
        indicadorEspecial.text = player.ValorEspecial().ToString();

        if (turno == "Player")
        {
            yield return new WaitForSeconds(5f);
            verificadorDeTurno = true;
            turno = "Inimigo";
        }
    }

    public void VerificaVitoria()
    {
        if (!inimigo.VerificaVida())
        {
            StartCoroutine(TelaVitoria());
        }
        else if (!player.VerificaVida())
        {
            textoTextoDerrota.SetActive(true);
        }
    }

    IEnumerator TelaVitoria()
    {
        yield return new WaitForSeconds(2.0f);
        yield return new WaitForSeconds(1.0f);
        textoTextoVitoria.SetActive(true);
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}