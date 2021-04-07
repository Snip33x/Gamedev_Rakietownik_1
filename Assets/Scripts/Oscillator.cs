using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    //RUSZANIE KLOCKIEM Z PUNKTU DO PUNKTU

    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)] float movementFactor; // Tak robimy slajdera w inspektorze - ten range to jest atrybut
    [SerializeField] float period = 2f;
   
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; //Debug.Log(startingPosition); możemy sobie tak np sprawdzić jaka jest pozycja tego obiektu w konsoli

    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } // jeden ze sposobów żeby nie mieć błędu NaN - zamiast porównywać do zera to do epsilona który jest super małym floatem - dobre praktyki

        float cycles = Time.time / period; // contunually growing over time

        const float tau = Mathf.PI * 2; // tau to jest jak bierzemy promień i kładziemy go na obwodzie koła - wtedy mieści się on ok. 6,28 raza
        float rawSinWave = Mathf.Sin(cycles * tau); // da nam to wynik między -1 a 1

        movementFactor = (rawSinWave + 1f) / 2f; // linia idzie od -1 do 1, tutaj dodajemy jedynke i wychodzi od 0 do 2 linia no i dzielimy przez 2


        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
