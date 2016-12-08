using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorTCP
{

    class Clienteacceso
    {
        private string ClientId;//Guarda el id del Cliente
        private bool isActive;// Estado si tiene Guardado algunCliente true tiene guardado alguncliente false no

        public Clienteacceso()
        {
            ClearCliente();
        }

        public void ClearCliente()
        {
            this.ClientId = "";
            this.isActive = false;
        }

        public bool get_isActive() { return isActive; }

        public string get_ClientId() { return ClientId; }

        public void Set_ClientID(string ClientID) { this.ClientId = ClientID; this.isActive = true; }

    }
}
