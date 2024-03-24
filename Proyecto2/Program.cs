// See https://aka.ms/new-console-template for more information

using System;

class Program
{
    static char[,] tablero = new char[6, 7]; // Tablero 6x7
    static char jugadorActual = 'X'; // Jugador actual (X o O)
    static Stack<int> historialMovimientos = new Stack<int>(); // Pila creada para mantener el historial de movimientos
    static Queue<char> turnosJugadores = new Queue<char>(); // Cola para mantener turnos de jugadores

    static void Main()
    {
        string nombreJugador1, nombreJugador2;

        Console.Write("Ingrese el nombre del Jugador 1: ");
        nombreJugador1 = Console.ReadLine();

        Console.Write("Ingrese el nombre del Jugador 2: ");
        nombreJugador2 = Console.ReadLine();

        InicializarTablero(); // Inicializa el tablero con espacios en blanco
        bool juegoGanado = false; // Indica si el juego ha sido ganado

        do
        {
            Console.Clear(); // Limpia la consola antes de mostrar el tablero
            MostrarTablero(); // Muestra el tablero actual
            int columna = ObtenerMovimiento(); // Obtiene la columna elegida por el jugador
            juegoGanado = RealizarMovimiento(columna); // Realiza el movimiento en la columna seleccionada

            if (!juegoGanado)
                CambiarJugador(); // Cambia al siguiente jugador si el juego no ha sido ganado

        } while (!TableroLleno() && !juegoGanado);  // Continúa el bucle hasta que el tablero esté lleno o el juego haya sido ganado

        Console.Clear(); // Limpia la consola antes de mostrar el tablero final
        MostrarTablero(); // Muestra el tablero final

        if (juegoGanado)
        {
            string nombreGanador = (jugadorActual == 'X') ? nombreJugador1 : nombreJugador2;
            Console.WriteLine($"¡Felicidades! Jugador {nombreGanador} ha ganado."); // Muestra el mensaje de victoria

            Console.Write("¿Desea jugar nuevamente? (S/N): ");
            string eleccion = Console.ReadLine().Trim().ToUpper();

            if (eleccion == "S")
            {
                ReiniciarJuego(); // Reinicia el juego
                Main(); // Vuelve a empezar el juego
            }
        }
        else
        {
            Console.WriteLine("¡Empate! El juego ha terminado."); // Muestra el mensaje de empate
            Console.WriteLine("Presione Enter para salir...");
            Console.ReadLine(); // Espera a que el usuario presione Enter para salir
        }
    }

    static void ReiniciarJuego()
    {
        InicializarTablero(); // Reinicia el tablero
        jugadorActual = 'X'; // Reinicia al jugador actual
       
    }


    static void InicializarTablero()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                tablero[i, j] = ' '; // Llena el tablero con espacios en blanco
            }
        }
    }

    static void MostrarTablero()
    {
        Console.WriteLine(" 0 1 2 3 4 5 6"); // Muestra los números de las columnas
        Console.WriteLine("---------------");

        for (int i = 0; i < 6; i++)
        {
            Console.Write("|");

            for (int j = 0; j < 7; j++)
            {
                Console.Write($"{tablero[i, j]}|"); // Muestra el contenido de cada celda del tablero
            }
            Console.WriteLine();
            Console.WriteLine("---------------");
        }
    }

    static int ObtenerMovimiento()
    {
        int columna;

        do
        {
            Console.Write($"Jugador {jugadorActual}, elige una columna (0-6): "); // Solicita al jugador que elija una columna
        } while (!int.TryParse(Console.ReadLine(), out columna) || columna < 0 || columna > 6 || !ColumnaDisponible(columna));// Verifica que la entrada sea válida y que la columna esté disponible

        return columna;
    }

    static bool ColumnaDisponible(int columna)
    {
        return tablero[0, columna] == ' '; // Verifica si la columna seleccionada está disponible
    }

    static bool RealizarMovimiento(int columna)
    {
        for (int i = 5; i >= 0; i--)
        {
            if (tablero[i, columna] == ' ')
            {
                tablero[i, columna] = jugadorActual; // Realiza el movimiento en la columna seleccionada
                historialMovimientos.Push(columna); // Agrega la columna al historial de movimientos
                return VerificarVictoria(i, columna); // Verifica si el movimiento resultó en una victoria
            }
        }

        return false;
    }

    static bool VerificarVictoria(int fila, int col)
    {
        int contador;

        // Verificación horizontal
        contador = ContarConsecutivas(fila, col, 0, 1) + ContarConsecutivas(fila, col, 0, -1) - 1;
        if (contador >= 4)
            return true;

        // Verificación vertical
        contador = ContarConsecutivas(fila, col, 1, 0) + ContarConsecutivas(fila, col, -1, 0) - 1;
        if (contador >= 4)
            return true;

        // Verificación diagonal derecha-abajo a izquierda-arriba
        contador = ContarConsecutivas(fila, col, 1, 1) + ContarConsecutivas(fila, col, -1, -1) - 1;
        if (contador >= 4)
            return true;

        // Verificación diagonal izquierda-abajo a derecha-arriba
        contador = ContarConsecutivas(fila, col, 1, -1) + ContarConsecutivas(fila, col, -1, 1) - 1;
        if (contador >= 4)
            return true;

        return false;
    }

    static int ContarConsecutivas(int fila, int col, int dirFila, int dirCol)
    {
        char jugadorObjetivo = tablero[fila, col];
        int contador = 0;

        while (fila >= 0 && fila < 6 && col >= 0 && col < 7 && tablero[fila, col] == jugadorObjetivo)
        {
            contador++;  // Incrementa el contador de celdas consecutivas ocupadas por el mismo jugador
            fila += dirFila;
            col += dirCol;
        }

        return contador;
    }

    static bool TableroLleno()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (tablero[i, j] == ' ')
                    return false; // Si encuentra una celda vacía, el tablero no está lleno
            }
        }
        return true; // Si no hay celdas vacías, el tablero está lleno
    }

    static void CambiarJugador()
    {
        if (turnosJugadores.Count == 0) // Si la cola de turnos está vacía, encola ambos jugadores
        {
            turnosJugadores.Enqueue('X');
            turnosJugadores.Enqueue('O');
        }

        jugadorActual = turnosJugadores.Dequeue(); // Desencola al siguiente jugador
    }
}