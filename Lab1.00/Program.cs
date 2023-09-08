using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1._00.Insertar;
using static Lab1._00.TreeNode;



namespace Lab1._00
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Talent hub de Guatemala");

                Console.WriteLine("0. Insertar registros");
                Console.WriteLine("1. Eliminar registros");
                Console.WriteLine("2. Actualizar un registro");
                Console.WriteLine("3. Buscar registros");
                Console.WriteLine("4. Recomendacion");
                Console.WriteLine("5. Salir");

                Console.WriteLine("Escoga una opcion del menú");

                int opcion = Convert.ToInt16(Console.ReadLine());

                Insertar datos = new Insertar(); //Crea una clase para insertar los datos del csv

                switch (opcion)
                {
                    case 0: // opcion para insertar los datos del csv 
                        Console.WriteLine("Insertar registros");
                        datos.InsertarDatosCSV();
                        break;

                    case 1: //opcion para eliminar un registro
                        Console.WriteLine("Eliminar registro");
                        datos.EliminarDatos();
                        break;

                    case 2: //opcion para actualizar un registro
                        Console.WriteLine("Actualizar un registro");
                        datos.ActualizarDatos();
                        break;

                    case 3: //opcion para buscar un registro
                        Console.WriteLine("Buscar registro asociado a un nombre");
                        datos.BuscarRegistros();
                        break;

                    case 4: //opcion para mostrar recomendaciones sobre la busqueda
                        Console.WriteLine("Recomendaciones");
                        Console.WriteLine(" Facilita la organizacion ");
                        Console.WriteLine(" Permite accceso por Indice ");
                        Console.WriteLine(" Facil implementacion ");
                        Console.WriteLine(" Buena eleccion para medianas cantidades de datos ");
                        break;
                    case 5:
                        Console.WriteLine("Saliendo...");

                        return;

                    default:
                        Console.WriteLine("Opción no válida. Ingrese un número del 0 al 5.");
                        break;
                }

                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }

        }
    }
}
