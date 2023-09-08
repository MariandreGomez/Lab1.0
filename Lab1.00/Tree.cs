using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab1._00.Insertar;
using System.IO;

namespace Lab1._00
{
    public class TreeNode
    {
        public persona Data { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(persona data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public TreeNode()
        {
        }

        private TreeNode root; // Raíz del árbol



        // Método para insertar un nodo en el árbol
        public void InsertarNodo(persona data)
        {
            root = InsertarArbol(root, data);
        }

        private TreeNode InsertarArbol(TreeNode node, persona data)
        {
            if (node == null)
            {
                return new TreeNode(data);
            }

            // Comparar para decidir si el nuevo nodo debe ir a la izquierda o derecha
            if (string.Compare(data.name, node.Data.name) < 0)
            {
                node.Left = InsertarArbol(node.Left, data);
            }
            else
            {
                node.Right = InsertarArbol(node.Right, data);
            }

            return node;
        }

        // Otros métodos como eliminar, buscar, etc.
        public List<persona> BuscarPorNombre(string nombre)
        {
            List<persona> personasConNombre = new List<persona>();
            BuscarPorNombreEnArbol(root, nombre, personasConNombre);
            return personasConNombre;
        }

        private void BuscarPorNombreEnArbol(TreeNode node, string nombre, List<persona> result)
        {
            if (node != null)
            {
                BuscarPorNombreEnArbol(node.Left, nombre, result);
                if (node.Data.name == nombre)
                {
                    result.Add(node.Data);
                }
                BuscarPorNombreEnArbol(node.Right, nombre, result);
            }
        }

        //actualizar
        public void ActualizarPersonasDesdeLista(List<persona> personasToUpdate)
        {
            foreach (var personToUpdate in personasToUpdate)
            {
                ActualizarPersona(root, personToUpdate);
            }
        }

        private void ActualizarPersona(TreeNode node, persona personToUpdate)
        {
            if (node == null)
            {
                return;
            }

            int comparacion = string.Compare(personToUpdate.name, node.Data.name);

            if (comparacion < 0)
            {
                // El nombre a actualizar es menor, buscar en el subárbol izquierdo
                ActualizarPersona(node.Left, personToUpdate);
            }
            else if (comparacion > 0)
            {
                // El nombre a actualizar es mayor, buscar en el subárbol derecho
                ActualizarPersona(node.Right, personToUpdate);
            }
            else
            {
                // El nodo actual contiene la persona a actualizar
                // Actualizar la información de la persona en el nodo
                node.Data.datebirth = personToUpdate.datebirth;
                node.Data.address = personToUpdate.address;
                // Puedes agregar más campos para actualizar según tus necesidades
            }
        }

        //buscar
        public List<persona> BuscarTodasPorNombre(string nombre)
        {
            List<persona> personasConNombre = new List<persona>();
            BuscarTodasPorNombreEnArbol(root, nombre, personasConNombre);
            return personasConNombre;
        }

        private void BuscarTodasPorNombreEnArbol(TreeNode nodoActual, string nombre, List<persona> result)
        {
            if (nodoActual == null)
            {
                return;
            }

            // Recursivamente buscar en ambos subárboles
            BuscarTodasPorNombreEnArbol(nodoActual.Left, nombre, result);

            // Si se encuentra una persona con el mismo nombre, agrégala a la lista de resultados
            if (string.Compare(nombre, nodoActual.Data.name) == 0)
            {
                result.Add(nodoActual.Data);
            }

            BuscarTodasPorNombreEnArbol(nodoActual.Right, nombre, result);
        }


        //eliminar
        public void EliminarDatosDesdeLista(List<persona> listaEliminar)
        {
            foreach (var personaEliminar in listaEliminar)
            {
                root = EliminarNodoPorNombre(root, personaEliminar.name);
            }
        }

        private TreeNode EliminarNodoPorNombre(TreeNode nodoActual, string nombre)
        {
            if (nodoActual == null)
            {
                return nodoActual;
            }

            // Recursivamente buscar el nodo a eliminar
            if (string.Compare(nombre, nodoActual.Data.name) < 0)
            {
                nodoActual.Left = EliminarNodoPorNombre(nodoActual.Left, nombre);
            }
            else if (string.Compare(nombre, nodoActual.Data.name) > 0)
            {
                nodoActual.Right = EliminarNodoPorNombre(nodoActual.Right, nombre);
            }
            else
            {
                // Si encontraste el nodo, procede a eliminarlo
                // Caso 1: Nodo sin hijos o con un solo hijo
                if (nodoActual.Left == null)
                {
                    return nodoActual.Right;
                }
                else if (nodoActual.Right == null)
                {
                    return nodoActual.Left;
                }

                // Caso 2: Nodo con dos hijos, encontrar el sucesor in-order
                nodoActual.Data = EncontrarMinimoValor(nodoActual.Right);

                // Elimina el nodo sucesor in-order
                nodoActual.Right = EliminarNodoPorNombre(nodoActual.Right, nodoActual.Data.name);
            }

            return nodoActual;
        }

        private persona EncontrarMinimoValor(TreeNode nodoActual)
        {
            persona minimo = nodoActual.Data;
            while (nodoActual.Left != null)
            {
                minimo = nodoActual.Left.Data;
                nodoActual = nodoActual.Left;
            }
            return minimo;
        }


        //mostrar arbol
        public void MostrarArbol()
        {
            Console.WriteLine("Contenido del árbol binario (Recorrido en orden):");
            MostrarArbolEnOrden(root);
        }

        private void MostrarArbolEnOrden(TreeNode node)
        {
            if (node != null)
            {
                MostrarArbolEnOrden(node.Left);

                if (node.Data.name == "aaliyah")
                {
                    Console.WriteLine("si hay");
                }

                Console.WriteLine($"Nombre: {node.Data.name}, DPI: {node.Data.dpi}, Fecha de Nacimiento: {node.Data.datebirth}, Dirección: {node.Data.address}");
                MostrarArbolEnOrden(node.Right);
            }
        }

        public int ContarNodos()
        {
            return ContarNodosRecursivo(root);
        }

        private int ContarNodosRecursivo(TreeNode nodoActual)
        {
            if (nodoActual == null)
            {
                return 0; // Si el nodo es nulo, no hay nodos que contar.
            }

            int izquierda = ContarNodosRecursivo(nodoActual.Left);
            int derecha = ContarNodosRecursivo(nodoActual.Right);

            // Sumar 1 para contar el nodo actual
            return izquierda + derecha + 1;
        }


    }
}
