using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Primera_calculadora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Propiedades
        private double _num = 0;


        // Constructor
        public MainWindow()
        {
            InitializeComponent();
        }



        // Metodos:


        // Numeros
        private void Numero_Click(object sender, RoutedEventArgs e)
        {

            // Verificamos si hay mensaje de error o si hay un cero
            if (textoNumero.Text.Contains("Error") || textoNumero.Text == "0")
            {
                textoNumero.Text = "";
            }

            // Obtenemos el número desde el contenido del botón
            Button botonNumero = (Button)sender;
            string? numero = botonNumero.Content.ToString();

            // Agregamos el número al texto
            textoNumero.Text += numero;

            // Convertimos el texto del textbox a double si es posible
            if (double.TryParse(numero, out double result))
            {
                _num = result;
            }

        }


        // Punto
        private void Punto_Click(object sender, RoutedEventArgs e)
        {

            // Verificamos si hay mensaje de error o si esta el cero inicial

            if (textoNumero.Text.Contains("Error"))
            {
                textoNumero.Text = "";
            }

            // Verificamos si el numero actual tiene un punto 
            if (!textoNumero.Text.EndsWith('.'))
            {
                textoNumero.Text += '.';
            }
        }


        // Operadores
        private void Operador_Click(object sender, RoutedEventArgs e)
        {

            // Lista a usar
            char[] operadores = { '+', '-', 'x', '÷' };


            // Obtenemos el objeto boton
            Button botonOperador = (Button)sender;


            // Obtenemos el contenido del boton y accedemos al caracter
            string? contenidoBoton = botonOperador.Content.ToString();


            // Accedemos al ultimo caracter del texto
            char ultimoDigito = textoNumero.Text.Length > 0 ? textoNumero.Text[^1] : '\0';


            // Verificamos si hay mensaje de error
            if (textoNumero.Text.Contains("Error"))
            {
                textoNumero.Text = "";
            }
            // Verificamos si esta el cero inicial y se digito un -
            else if (textoNumero.Text.EndsWith('0') && contenidoBoton == "-")
            {
                textoNumero.Text = textoNumero.Text.TrimStart('0');
            }

            // Verificamos si el ultimo digito es un operador
            if (operadores.Contains(ultimoDigito))
            {
                textoNumero.Text = string.Concat(textoNumero.Text.AsSpan(0, textoNumero.Text.Length - 1), contenidoBoton);
            }
            else
            {
                textoNumero.Text += contenidoBoton;
            }
           

        }


        // Borrar 
        private void Borrar_Click(object sender, RoutedEventArgs e)
        {

            // Convertimos el sender a un objeto Button
            Button botonNumero = (Button)sender;


            // Verificamos si es C, si ya se borro el texto o hay mensaje de error
            if (textoNumero.Text.Contains("Error") || botonNumero.Content.Equals("C") || textoNumero.Text.Length == 1)
            {
                textoNumero.Text = "0";
            }
            // Borramos el ultimo digito
            else if (textoNumero.Text.Length > 1)
            {
                textoNumero.Text = textoNumero.Text.Remove(textoNumero.Text.Length - 1);

            }
        }


        // Igual
        private void Igual_Click(object sender, RoutedEventArgs e)
        {

            // Lista a usar
            char[] operadores = { '+', '-', 'x', '÷' };

            // Obtenenos la expresión matemática completa
            string expresion = textoNumero.Text;

            // Utilizamos DataTable para evaluar la expresión
            DataTable table = new();

            try
            {
                // Verificamos si la expresión es solo un punto
                if (expresion == ".")
                {
                    throw new SyntaxErrorException("Error de sintaxis");
                }
                // Verificamos si la expresión termina con un operador
                else if (operadores.Contains(expresion.Last()))
                {
                    throw new SyntaxErrorException("La expresión no puede terminar con un operador.");
                }
                // Verificamos si la expresión empieza con un operador
                else if (operadores.Contains(expresion[0]))
                {
                    throw new SyntaxErrorException("La expresión no puede comenzar con un operador.");
                }
                else
                {
                    // Reemplazamos los símbolos personalizados por los operadores reconocidos por DataTable
                    expresion = expresion.Replace('x', '*').Replace('÷', '/');

                    // Calculamos el resultado
                    var resultado = table.Compute(expresion, "");

                    // Mostramos el resultado
                    textoNumero.Text = resultado.ToString();
                }

            }
            catch (Exception)
            {
                // Manejamos otras excepciones no especificadas
                textoNumero.Text = "Error de sintaxis";
            }
        }

    }
}
