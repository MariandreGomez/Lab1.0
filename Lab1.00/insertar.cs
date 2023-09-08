using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using static Lab1._00.TreeNode;

namespace Lab1._00
{
    public class CSVData //clase para leer el archivo csv
    {
        public CSVData()
        {
        }

        public CSVData(string v1, string v2)
        {
            operacion = v1;
            JSONData = v2;
        }

        public string operacion { get; set; }
        public string JSONData { get; set; }

    }

    public class persona //clase para leer el json 
    {
        public persona(string nameC, string dpiC, string datebirthC, string adrressC)
        {
            name = nameC;
            dpi = dpiC;
            datebirth = datebirthC;
            address = adrressC;
        }
        public string name { get; set; }
        public string dpi { get; set; }
        public string datebirth { get; set; }
        public string address { get; set; }
    }

    internal class Insertar
    {

        public static List<CSVData> insert = new List<CSVData>(); //lista donde se inserta la operacion "INSERT" y JSONdata
        public static List<CSVData> delete = new List<CSVData>(); //lista donde se guarda los elementos a eliminar 
        public static List<CSVData> patch = new List<CSVData>(); //lista donde se guarda la informacion a actualizar
        public static List<persona> JsonDataInsert = new List<persona>(); //lista donde se inserta el json y se modifica
        public static List<persona> JsonDataDelete = new List<persona>(); //lista donde se inserta lo que se quiere eliminar
        public static List<persona> JsonDataPatch = new List<persona>(); //lista donde se inserta lo que se quiere actualizar
        TreeNode insertarObjeto = new TreeNode(); //arbol 

        //metodo para insertar los datos en el CSV 
        public void InsertarDatosCSV() //metodo para insertar datos
        {

            string[] CsvLines = System.IO.File.ReadAllLines(@"C:\Users\maria\Desktop\datos.csv"); //guado los datos del csv en un arreglo


            for (int i = 0; i < CsvLines.Length; i++) //for que recorre el arreglo
            {
                // CsvLines[i] = CsvLines[i].Replace("\"\"", "\"");

                string[] rowdata = CsvLines[i].Split(';'); // lee el separador ";" 
                CSVData record = new CSVData(rowdata[0], rowdata[1]); //se inserta en la clase que contiene el jsondata y la operacion


                if (rowdata[0] == "INSERT")
                {
                    insert.Add(record);


                }
                else if (rowdata[0] == "DELETE")
                {
                    delete.Add(record);
                }
                else if (rowdata[0] == "PATCH")
                {
                    patch.Add(record);
                }



            }


            foreach (CSVData item in insert)
            {
                persona person = JsonConvert.DeserializeObject<persona>(item.JSONData);
                JsonDataInsert.Add(person);
                //insertar en arbolAVL
                insertarObjeto.InsertarNodo(person);

            }

            Console.WriteLine("Los datos se han insertado con exito");



        }

        //metodo para eliminar los datos de una persona
        public void EliminarDatos()
        {
            //se dezerialza el json donde se encuentra la operacion "Eliminar"
            foreach (CSVData item in delete)
            {
                persona person = JsonConvert.DeserializeObject<persona>(item.JSONData);
                JsonDataDelete.Add(person);
            }

            //se compara la lista JsonDataInsert donde estan los datos que se insertaron
            //con la lista JsonDataDelete y se eliminan en la lista JsonDataInsert
            foreach (var itemDelete in JsonDataDelete)
            {
                JsonDataInsert.RemoveAll(persona =>
                   persona.name == itemDelete.name && persona.dpi == itemDelete.dpi);

            }

            Console.WriteLine("se han eliminado los datos con exito");

            //eliminar nodo arbol
            insertarObjeto.EliminarDatosDesdeLista(JsonDataDelete);

            //se muestran la cantidad de datos que quedaron en JsonDataInsert despues de la eliminacion
            /*   int cantdatos = 0;
               foreach (var persona in JsonDataInsert)
               {
                   cantdatos = cantdatos + 1;
                   Console.WriteLine($"Nombre: {persona.name}, DPI: {persona.dpi}, Fecha de nacimiento: {persona.datebirth}, Dirección: {persona.address}");
               }
               Console.WriteLine("han quedado " + Convert.ToString(cantdatos) + " candidatos");
            */
        }

        //metodo para actualizar datos de una persona 
        public void ActualizarDatos()
        {
            //se dezerialza el json donde se encuentra la operacion "Patch"
            foreach (CSVData item in patch)
            {
                persona person = JsonConvert.DeserializeObject<persona>(item.JSONData);
                JsonDataPatch.Add(person);
            }

            //se compara la lista JsonDataInsert donde estan los datos que se insertaron
            //con la lista JsonDataDelete y se eliminan en la lista JsonDataInsert
            foreach (var itemPatch in JsonDataPatch)
            {
                var personaAactualizar = JsonDataInsert.FirstOrDefault(p => p.name == itemPatch.name && p.dpi == itemPatch.dpi);
                if (personaAactualizar != null)
                {
                    personaAactualizar.datebirth = itemPatch.datebirth;
                    personaAactualizar.address = itemPatch.address;
                }
            }
            //actualizar arbol
            insertarObjeto.ActualizarPersonasDesdeLista(JsonDataPatch);

            //mostrar las actualizaciones
            /*  foreach (var persona in JsonDataInsert)
              {
                  Console.WriteLine($"Nombre: {persona.name}, DPI: {persona.dpi}, Fecha de Nacimiento: {persona.datebirth}, Dirección: {persona.address}");
              }
            */

            Console.WriteLine("Se han realizo las actulizaciones necesarias");
        }

        //metodo para buscar a una persona por nombre 
        public void BuscarRegistros()
        {

            Console.WriteLine("Que nombre desea buscar");
            string nombre = Console.ReadLine();
            var nameaux = JsonDataInsert.Where(p => p.name == nombre).ToList(); //realiza busqueda y lo guarda en una lista auxiliar





            //muestra todo lo que se encontro en la busqueda
            if (nameaux.Any())
            {

                foreach (var item in nameaux)
                {
                    Console.WriteLine($"Nombre: {item.name}");
                    Console.WriteLine($"DPI: {item.dpi}");
                    Console.WriteLine($"Fecha de Nacimiento: {item.datebirth}");
                    Console.WriteLine($"Dirección: {item.address}");
                    Console.WriteLine("-------------------------------");

                }


                string JsonText = JsonConvert.SerializeObject(nameaux, Formatting.Indented);
                string JsonFile = "Busqueda.json";
                File.WriteAllText(JsonFile, JsonText);
                Console.WriteLine($"Los registros encontrados se han guardado en Busqueda.json.");
            }
            else
            {
                Console.WriteLine("No se encontro ninguna persona");
            }



        }
    }
}
