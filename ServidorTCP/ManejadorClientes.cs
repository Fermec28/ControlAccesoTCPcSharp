using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorTCP
{
    class ManejadorClientes
    {
        private bool Activo;// True si aun quedan Clientes por Guardar false si no
        private List<Clienteacceso> Clientes;

        public ManejadorClientes(int CantidadClientes)
        {
            this.Activo = true;
            Clientes = new List<Clienteacceso>();
            for (int i = 0; i < CantidadClientes; i++)
            {
                Clientes.Add(new Clienteacceso());
            }
        }

        public bool IsActivo() { return Activo; }

        public void InsertCliente(String Id)//se puede mejorar el modo de las banderas
        {
            int ClientesActivos = Clientes.Count, iteracion = 0;
            Console.WriteLine("se a creado el cliente" + Id);

            foreach (Clienteacceso aux in Clientes)
            {
                iteracion++;
                if (Activo && !aux.get_isActive())
                { //pregunta si el manejador esta activo y si queda algun cupo libre para guardar
                    aux.Set_ClientID(Id);// Establece el Id del Cliente                    
                    break;
                }
                else { ClientesActivos--; }
            }
            this.Activo = (ClientesActivos == 0 || iteracion == Clientes.Count) ? false : true; // si no hay cupo libre en ninguno de la lista clientes el manejador es falso

        }

        public void ClearClientes()
        {
            //Este metodo me permite limpiar Los datos del Cliente
            foreach (Clienteacceso aux in Clientes)
            {
                aux.ClearCliente();
            }
            Console.WriteLine("\n Clientes limpiados");
            this.Activo = true;
        }

        public bool IsDiferent()
        {// retorna true si son diferentes los Id false si hay almenos uno igual
            
            bool IsDiferentID = true;

            if (Clientes.Count > 1)
            {
                for (int i = 0; i < Clientes.Count - 1 && IsDiferentID; i++)
                {
                    string clave = Clientes[i].get_ClientId();
                    for (int j = i + 1; j < Clientes.Count && IsDiferentID; j++)
                    {

                        IsDiferentID = (clave != Clientes[j].get_ClientId()) ? true : false;
                    }
                }
            }
            else
            {
                IsDiferentID = true;
            }
            return IsDiferentID;
        }        
    }
}
