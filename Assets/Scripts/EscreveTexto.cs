using System.Collections;
using UnityEngine;
using TMPro;

public class EscreveTexto : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto; // Referência ao componente TextMeshProUGUI
    [TextArea] // Campo de texto para editar a mensagem completa no Inspector
    [SerializeField] private string mensagemCompleta; // Texto completo a ser exibido
    [SerializeField] private float velocidadeDigitacao = 0.05f; // Tempo entre letras
    private AudioSource tocadorSom;
    [SerializeField] AudioClip som;


    private bool escrevendo = false;

    void Start()
    {
        tocadorSom = GetComponent<AudioSource>(); // Obtém o componente AudioSource
        texto.text = ""; // Limpa o texto atual
        StartCoroutine(DigitarTexto());
    }

    private IEnumerator DigitarTexto() // Coroutina para digitar o texto
    {
        escrevendo = true;

        foreach (char letra in mensagemCompleta) // Itera sobre cada letra da mensagem
        {
            texto.text += letra;
            tocadorSom.PlayOneShot(som); // Toca o som de digitação
            yield return new WaitForSeconds(velocidadeDigitacao); // Espera antes de escrever a próxima letra
        }

        escrevendo = false;
    }
}
